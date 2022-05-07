using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetuserWithLikes(int userId);

        Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate, int userId);
    }
}