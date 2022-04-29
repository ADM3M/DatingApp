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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        private (byte[] pwdHash, byte[] salt) GeneratePassword(string pwd)
        {
            using HMACSHA512 sha = new();

            var pwdHash = sha.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            return (pwdHash: pwdHash, salt: sha.Key);
        }

        private (byte[] pwdHash, byte[] salt) GeneratePassword(string pwd, byte[] salt)
        {
            using HMACSHA512 sha = new(salt);

            var pwdHash = sha.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            return (pwdHash: pwdHash, salt: sha.Key);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDto)
        {
            if (await UserExists(registerDto.username))
            {
                return BadRequest("Username is taken");
            }

            var pwdData = GeneratePassword(registerDto.pwd);

            AppUser user = new()
            {
                Name = registerDto.username,
                PasswordHash = pwdData.pwdHash,
                PasswordSalt = pwdData.salt
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDTO()
            {
                Username = user.Name,
                Token = tokenService.GenereteToken(user)
            };
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
                .SingleOrDefaultAsync(x => x.Name == dto.username);

            if (user is null)
            {
                return BadRequest("Incorrect username or password");
            }

            var pwdData = GeneratePassword(dto.pwd, user.PasswordSalt);

            for (int i = 0; i < user.PasswordHash.Length; i++)
            {
                if (user.PasswordHash[i] != pwdData.pwdHash[i])
                {
                    return BadRequest("Incorrect username or password");
                }
            }

            return new UserDTO()
            {
                Username = user.Name,
                Token = tokenService.GenereteToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
            };
        }
    }
}