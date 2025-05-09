using AutoMapper;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;

namespace DatingApp.Services
{
    public class MessagesService(IUnitOfWork unitOfWork, IMapper mapper) : IMessagesService
    {
        public async Task<MessageDto?> CreateMessage(string username, CreateMessageDto createMessageDto)
        {
            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new ArgumentException("You cannot message yourself");

            var sender = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null)
                return null;

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content,
            };

            unitOfWork.MessageRepository.Add(message);

            if (await unitOfWork.Complete())
                return mapper.Map<MessageDto>(message);

            return null;
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            return await unitOfWork.MessageRepository.GetMessagesForUser(messageParams);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            return await unitOfWork.MessageRepository.GetMessageThread(currentUsername, recipientUsername);
        }

        public async Task<bool> DeleteMessage(string username, int messageId)
        {
            var message = await unitOfWork.MessageRepository.GetByIdAsync(messageId);

            if (message == null) return false;

            if (message.SenderUsername != username && message.RecipientUsername != username)
                throw new UnauthorizedAccessException();

            if (message.SenderUsername == username) message.SenderDeleted = true;
            if (message.RecipientUsername == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                unitOfWork.MessageRepository.Delete(message);
            }

            return await unitOfWork.Complete();
        }
    }
}