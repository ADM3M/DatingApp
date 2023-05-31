using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UnitOfWork(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public IUserRepository UserReposiroty => new UserRepository(_context, _mapper);
        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);
        public ILikesRepository LikesRepository => new LikesRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool hasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}