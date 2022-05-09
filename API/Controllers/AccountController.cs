using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDto)
        {
            if (await UserExists(registerDto.Name))
            {
                return BadRequest("Name is taken");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            user.Name = registerDto.Name.ToLower();

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = new UserDTO()
            {
                Name = user.Name,
                Token = tokenService.GenereteToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
            
            return result;
        }

        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x => x.Name.ToLower() == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO dto)
        {
            var user = await context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Name == dto.Name);

            if (user is null)
            {
                return BadRequest("Incorrect username or password");
            }

            return new UserDTO()
            {
                Name = user.Name,
                Token = tokenService.GenereteToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }
    }
}