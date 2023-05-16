using System.Threading.Tasks;
using API.Entities;
using API.Services.Models;

namespace API.Interfaces;

public interface IFavoriteUserService
{
    public Task<OperationResult<UserLike>> AddToFavorite(int currentUserId, int targetUserId);

    public Task<OperationResult<UserLike>> RemoveFromFavorites(int currentUserId, int targetUserId);
}