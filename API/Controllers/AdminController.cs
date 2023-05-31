using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Name = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList(),
                })
                .ToListAsync();
            
            return Ok(users);
        }
        
        [HttpPost("edit-roles/{name}")]
        public async Task<ActionResult> EditRoles(string name, [FromQuery] string roles)
        {
            if (roles is null) return BadRequest("User cannot have no roles");
            
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(name);

            if (user is null) return NotFound("Could not find user");

            if (user.UserName.ToLower() == "admin") return BadRequest("You can't unpermit administrator!");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators cansee this");
        }
    }
}