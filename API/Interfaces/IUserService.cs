using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces;

public interface IUserService
{
    Task<IReadOnlyCollection<int>> GetUserFavoriteUserIds(int userId);
}