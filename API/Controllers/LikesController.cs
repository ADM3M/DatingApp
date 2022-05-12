using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string userName)
        {
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(User.GetUserId());
            
            if (sourceUser.UserName == userName) return BadRequest("You can't like yourself");
            
            var likedUser = await _unitOfWork.UserReposiroty.GetUserByUserNameAsync(userName);
            if (likedUser is null) return NotFound();

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUser.Id, likedUser.Id);

            if (userLike is not null)
            {
                return BadRequest("User has already been liked");
            }

            userLike = new Entities.UserLike
            {
                SourceUserId = sourceUser.Id,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var userLikes = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);
            
            Response.AddPaginationHeader(userLikes.CurrentPage,
                userLikes.PageSize, userLikes.TotalCount, userLikes.TotalPages);
            
            return Ok(userLikes);
        }

    }
}