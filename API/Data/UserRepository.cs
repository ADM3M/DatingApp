using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDTO> GetMemberAsync(string name)
        {
            return await _context.Users
                .Where(x => x.UserName == name)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public PagedList<MemberDTO> GetMembers(UserParams userParams)
        {
            IEnumerable<AppUser> users = _context.Users
                .Include(u => u.UserRoles)
                .Include(u => u.Photos)
                .ToList()
                .Where(u => CheckIfCurrentUserOrAdmin(u, userParams.CurrentUserName));

            if (!string.IsNullOrWhiteSpace(userParams.EmployeeName))
            {
                users = users.Where(u =>
                    u.KnownAs.Contains(userParams.EmployeeName, StringComparison.CurrentCultureIgnoreCase)
                    || u.UserName.Contains(userParams.EmployeeName, StringComparison.CurrentCultureIgnoreCase));
            }
            
            if (!string.IsNullOrWhiteSpace(userParams.Department))
            {
                users = users.Where(u =>
                    u.Department.Contains(userParams.Department, StringComparison.CurrentCultureIgnoreCase));
            }
            
            users = userParams.OrderBy switch
            {
                "created" => users.OrderBy(u => u.Created),
                _ => users.OrderByDescending(u => u.LastActive),
            };

            return PagedList<MemberDTO>.Create(_mapper.Map<IReadOnlyCollection<MemberDTO>>(users.ToList()),
                    userParams.PageNumber, userParams.PageSize);
        }

        private bool CheckIfCurrentUserOrAdmin(AppUser appUser, string currentUsername)
        {
            var isCurrentUser = appUser.UserName == currentUsername;

            var isAdmin = appUser.UserRoles != null && appUser.UserRoles.Any(ur => ur.RoleId != 1);
            
            return !isCurrentUser && !isAdmin;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users
            .FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string name)
        {
            return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(user => user.UserName == name);
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _context.Users.Where(x => x.UserName == username)
                .Select(x => x.Gender)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}