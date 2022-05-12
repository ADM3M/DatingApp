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

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query
                .Where(u => u.UserName != userParams.CurrentUserName)
                .Where(u => u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth <= maxDob && u.DateOfBirth >= minDob);
            query = userParams.OrderBy switch
            {
                "created" => query.OrderBy(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive),
            };

            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(_mapper
                .ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber, userParams.PageSize);
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