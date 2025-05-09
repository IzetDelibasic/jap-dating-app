using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Services.Interfaces
{
    public interface IMessagesService
    {
        Task<MessageDto?> CreateMessage(string username, CreateMessageDto createMessageDto);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);
        Task<bool> DeleteMessage(string username, int messageId);
    }
}