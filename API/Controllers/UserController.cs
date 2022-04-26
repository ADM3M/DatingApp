using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsersAsync());
        }

        [HttpGet]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user =  await _userRepository.GetUserByIdAsync(id);
            
            if (user is null)
            {
                return BadRequest("User not found");
            }

            return user;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<AppUser>> GetUser(string name)
        {
            var user =  await _userRepository.GetUserByUserName(name);
            
            if (user is null)
            {
                return BadRequest("User not found");
            }

            return user;
        }
    }
}