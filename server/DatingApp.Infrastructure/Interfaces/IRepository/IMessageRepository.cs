using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Repository.Interfaces;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
    Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection?> GetConnection(string connectionId);
    Task<Group?> GetMessageGroup(string groupName);
    Task<Group?> GetGroupForConnection(string connectionId);
}