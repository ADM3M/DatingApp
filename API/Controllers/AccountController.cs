using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService tokenService;

        private readonly IMapper _mapper;

        private readonly UserManager<AppUser> _userManger;

        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManger, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            this.tokenService = tokenService;
            this._userManger = userManger;
            this._signInManager = signInManager;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDto)
        {
            if (await UserExists(registerDto.Name))
            {
                return BadRequest("Name is taken");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Name.ToLower();

            var result = await _userManger.CreateAsync(user, registerDto.pwd);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            return new UserDTO()
            {
                Name = user.UserName,
                Token = tokenService.GenereteToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManger.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userManger.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Name.ToLower());

            if (user is null)
            {
                return BadRequest("Invalid username or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Pwd, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDTO()
            {
                Name = user.UserName,
                Token = tokenService.GenereteToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }
    }
}