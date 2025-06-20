using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;
using DatingApp.Helpers;

namespace DatingApp.Repository.Interfaces;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<PagedList<MessageResponse>> GetMessagesForUser(MessageParams messageParams);
    Task<IEnumerable<MessageResponse>> GetMessageThread(string currentUsername, string recipientUsername);
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection?> GetConnection(string connectionId);
    Task<Group?> GetMessageGroup(string groupName);
    Task<Group?> GetGroupForConnection(string connectionId);
}