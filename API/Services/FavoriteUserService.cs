using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class FavoriteUserService : IFavoriteUserService
{
    private readonly DataContext _dataContext;
    private readonly IUnitOfWork _uow;

    public FavoriteUserService(DataContext dataContext, IUnitOfWork uow)
    {
        _dataContext = dataContext;
        _uow = uow;
    }
    
    public async Task<OperationResult<UserLike>> AddToFavorite(int currentUserId, int targetUserId)
    {
        var userLike = new UserLike
        {
            SourceUserId = currentUserId,
            LikedUserId = targetUserId
        };

        await _dataContext.Likes.AddAsync(userLike);
        var operationResult = await _uow.Complete();

        return new OperationResult<UserLike>(operationResult, userLike);
    }

    public async Task<OperationResult<UserLike>> RemoveFromFavorites(int currentUserId, int targetUserId)
    {
        var entity = await _dataContext.Likes
            .SingleOrDefaultAsync(l => l.SourceUserId == currentUserId && l.LikedUserId == targetUserId);

        if (entity is null)
        {
            return new OperationResult<UserLike>(false, null);
        }

        _dataContext.Likes.Remove(entity);
        var operationResult = await _uow.Complete();

        return new OperationResult<UserLike>(operationResult, entity);
    }
}