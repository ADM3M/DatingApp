using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyCollection<int>> GetUserFavoriteUserIds(int userId)
    {
        var favoriteUsers = await _context.Likes
            .Where(u => u.SourceUserId == userId)
            .Select(ul => ul.LikedUserId)
            .ToListAsync();

        return favoriteUsers;
    }
}