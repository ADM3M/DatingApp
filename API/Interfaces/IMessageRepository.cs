using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);

        void RemoveConnection(Connection connection);

        Task<Connection> GetConnection(string connectionId);

        Task<Group> GetMessageGroup(string groupName);

        Task<Group> GetFroupForConnection(string connectionId);
        
        void AddMessage(Message message);

        void DeleteMessage(Message message);

        Task<Message> GetMessage(int id);

        Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);

        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentName, string recipientName);

        Task<bool> SaveAllAsync();
    }
}