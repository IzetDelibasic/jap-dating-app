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
    public void AddGroup(Group group)
    {
        db.Groups.Add(group);
    }

    public void AddMessage(Message message)
    {
        db.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        db.Messages.Remove(message);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await db.Connections.FindAsync(connectionId);
    }

    public async Task<Group?> GetGroupForConnection(string connectionId)
    {
        return await db.Groups
        .Include(x => x.Connections)
        .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
        .FirstOrDefaultAsync();
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await db.Messages.FindAsync(id);
    }

    public async Task<Group?> GetMessageGroup(string groupName)
    {
        return await db.Groups
            .Include(x => x.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName);
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
        var query = db.Messages
            .Where(x =>
            x.RecipientUsername == currentUsername
                && x.RecipientDeleted == false
                && x.SenderUsername == recipientUsername ||
            x.SenderUsername == currentUsername
                && x.SenderDeleted == false
                && x.RecipientUsername == recipientUsername
            )
            .OrderBy(x => x.MessageSent)
            .AsQueryable();

        var unreadMessages = query.Where(x => x.DateRead == null &&
            x.RecipientUsername == currentUsername).ToList();

        if (unreadMessages.Count != 0)
        {
            unreadMessages.ForEach(x => x.DateRead = DateTime.UtcNow);
        }

        return await query.ProjectTo<MessageDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    public void RemoveConnection(Connection connection)
    {
        db.Connections.Remove(connection);
    }
}
