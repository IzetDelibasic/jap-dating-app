using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository;

public class MessageRepository(DataContext db, IMapper mapper) : IMessageRepository
{
    public void AddMessage(Message message)
    {
        db.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        db.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await db.Messages.FindAsync(id);
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = db.Messages
            .OrderByDescending(x => x.MessageSent)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.Username
                && x.RecipientDeleted == false),
            "Outbox" => query.Where(x => x.Sender.UserName == messageParams.Username
                && x.SenderDeleted == false),
            _ => query.Where(x => x.Recipient.UserName == messageParams.Username && x.DateRead == null
                && x.RecipientDeleted == false)
        };

        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

        return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await db.Messages
            .Include(x => x.Sender).ThenInclude(x => x.Photos)
            .Include(x => x.Recipient).ThenInclude(x => x.Photos)
            .Where(x =>
            x.RecipientUsername == currentUsername
                && x.RecipientDeleted == false
                && x.SenderUsername == recipientUsername ||
            x.SenderUsername == currentUsername
                && x.SenderDeleted == false
                && x.RecipientUsername == recipientUsername
            )
            .OrderBy(x => x.MessageSent)
            .ToListAsync();

        var unreadMessages = messages.Where(x => x.DateRead == null &&
            x.RecipientUsername == currentUsername).ToList();

        if (unreadMessages.Count != 0)
        {
            unreadMessages.ForEach(x => x.DateRead = DateTime.UtcNow);
            await db.SaveChangesAsync();
        }

        return mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await db.SaveChangesAsync() > 0;
    }
}
