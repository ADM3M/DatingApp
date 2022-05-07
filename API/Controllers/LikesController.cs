using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string userName)
        {
            var sourceUser = await _likesRepository.GetUserWithLikes(User.GetUserId());
            
            if (sourceUser.Name == userName) return BadRequest("You can't like yourself");
            
            var likedUser = await _userRepository.GetUserByUserNameAsync(userName);
            if (likedUser is null) return NotFound();

            var userLike = await _likesRepository.GetUserLike(sourceUser.Id, likedUser.Id);

            if (userLike is not null)
            {
                // sourceUser.LikedUsers.Remove(userLike);
                // return await _userRepository.SaveAllAsync() ? Ok()
                // : BadRequest("Error while deleting user like");
                return BadRequest("User has already been liked");
            }

            userLike = new Entities.UserLike
            {
                SourceUserId = sourceUser.Id,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes(string predicate)
        {
            var userLikes = await _likesRepository.GetUserLikes(predicate, User.GetUserId());
            return Ok(userLikes);
        }

    }
}