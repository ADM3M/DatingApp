using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPhotoService photoService,
            IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery] UserParams userParams)
        {
            var gender = await _unitOfWork.UserReposiroty.GetUserGender(User.GetUserName());

            userParams.CurrentUserName = User.GetUserName();

            var users = await _unitOfWork.UserReposiroty.GetMembersAsync(userParams);

            if (!string.IsNullOrWhiteSpace(userParams.Department))
            {
                var usersFilteredByDepartment = users.Where(u =>
                    u.Department.Contains(userParams.Department, StringComparison.CurrentCultureIgnoreCase));
                users = PagedList<MemberDTO>.Create(usersFilteredByDepartment, userParams.PageNumber, userParams.PageSize);
            }

            if (!string.IsNullOrWhiteSpace(userParams.EmployeeName))
            {
                var usersFilteredByDepartment = users.Where(u =>
                    u.Name.Contains(userParams.EmployeeName, StringComparison.CurrentCultureIgnoreCase));
                users = PagedList<MemberDTO>.Create(usersFilteredByDepartment, userParams.PageNumber, userParams.PageSize);
            }

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }

        [HttpGet("details")]
        public async Task<ActionResult<IEnumerable<MemberWithDetailsDTO>>> GetUserWithDetails(
            [FromQuery] UserParams userParams)
        {
            var gender = await _unitOfWork.UserReposiroty.GetUserGender(User.GetUserName());

            userParams.CurrentUserName = User.GetUserName();
            var userId = User.GetUserId();

            var members = await _unitOfWork.UserReposiroty.GetMembersAsync(userParams);
            
            if (!string.IsNullOrWhiteSpace(userParams.Department))
            {
                var usersFilteredByDepartment = members.Where(u =>
                    u.Department.Contains(userParams.Department, StringComparison.CurrentCultureIgnoreCase));
                members = PagedList<MemberDTO>.Create(usersFilteredByDepartment, userParams.PageNumber, userParams.PageSize);
            }

            if (!string.IsNullOrWhiteSpace(userParams.EmployeeName))
            {
                var usersFilteredByDepartment = members.Where(u =>
                    u.Name.Contains(userParams.EmployeeName, StringComparison.CurrentCultureIgnoreCase));
                members = PagedList<MemberDTO>.Create(usersFilteredByDepartment, userParams.PageNumber, userParams.PageSize);
            }

            var favoriteUserIds = await _userService.GetUserFavoriteUserIds(userId);

            Response.AddPaginationHeader(members.CurrentPage, members.PageSize, members.TotalCount, members.TotalPages);

            var membersWithDetails = members.Select(CreateFrom);
            
            return Ok(membersWithDetails);
            
            MemberWithDetailsDTO CreateFrom(MemberDTO member)
            {
                var memberWithDetails = _mapper.Map<MemberWithDetailsDTO>(member);
                memberWithDetails.IsFavorite = favoriteUserIds.Contains(member.Id);

                return memberWithDetails;
            }
        }

        [HttpGet("{name}", Name = "GetUser")]
        public async Task<ActionResult<MemberDTO>> GetUser(string name)
        {
            return await _unitOfWork.UserReposiroty.GetMemberAsync(name);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var username = User.GetUserName();
            var user = await _unitOfWork.UserReposiroty.GetUserByUserNameAsync(username);

            _mapper.Map(memberUpdateDTO, user);

            _unitOfWork.UserReposiroty.Update(user);

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserReposiroty.GetUserByUserNameAsync(User.GetUserName());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error is not null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = user.Photos.Count == 0,
            };

            user.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetUser", new { name = user.UserName }, _mapper.Map<PhotoDTO>(photo));
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _unitOfWork.UserReposiroty.GetUserByUserNameAsync(User.GetUserName());

            var photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);

            if (photo.IsMain)
            {
                return BadRequest("this is already your main photo");
            }

            var currentMain = user.Photos.FirstOrDefault(photo => photo.IsMain);
            if (currentMain is not null)
            {
                currentMain.IsMain = false;
            }

            photo.IsMain = true;

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.UserReposiroty.GetUserByUserNameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo is null)
            {
                return NotFound();
            }

            if (photo.IsMain)
            {
                return BadRequest("you can't delete main photo");
            }

            if (photo.PublicId is not null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error is not null)
                {
                    return BadRequest(result.Error.Message);
                }
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("failed to delete the photo");
        }
    }
}