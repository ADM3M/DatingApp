using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Enums;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFavoriteUserService _favoriteUserService;

        public LikesController(IUnitOfWork unitOfWork, IFavoriteUserService favoriteUserService)
        {
            _unitOfWork = unitOfWork;
            _favoriteUserService = favoriteUserService;
        }

        [HttpPost("{targetUserId}")]
        public async Task<ActionResult<LikeState>> AddLike(int targetUserId)
        {
            var currentUserId = User.GetUserId();

            var targetUserExists = await _unitOfWork.UserReposiroty.UserExistsAsync(targetUserId);

            if (!targetUserExists)
            {
                return BadRequest("Target user doesn't exists");
            }

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(currentUserId, targetUserId);

            OperationResult<UserLike> operationResult;
            LikeState likeState;
            if (userLike is null)
            {
                operationResult = await _favoriteUserService.AddToFavorite(currentUserId, targetUserId);
                likeState = LikeState.Liked;
            }
            else
            {
                operationResult = await _favoriteUserService.RemoveFromFavorites(currentUserId, targetUserId);
                likeState = LikeState.Unliked;
            }

            if (!operationResult.Success)
            {
                return BadRequest("Error while adding user to favorites");
            }

            return Ok(likeState);
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