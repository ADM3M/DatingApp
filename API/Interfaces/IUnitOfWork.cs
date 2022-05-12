using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserReposiroty { get;}

        public IMessageRepository MessageRepository { get;}

        public ILikesRepository LikesRepository { get;}

        Task<bool> Complete();

        bool hasChanges();
    }
}