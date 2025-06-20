using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository;

public class MessageRepository(DatabaseContext dbContext, IMapper mapper) : BaseRepository<Message>(dbContext), IMessageRepository
{
    private readonly DatabaseContext _dbContext = dbContext; 
    private readonly IMapper _mapper = mapper;

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await _dbContext.Connections.FindAsync(connectionId);
    }

    public async Task<Group?> GetGroupForConnection(string connectionId)
    {
        return await _dbContext.Groups
            .Include(x => x.Connections)
            .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
    }

    public async Task<Group?> GetMessageGroup(string groupName)
    {
        return await _dbContext.Groups
            .Include(x => x.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName);
    }

    public async Task<PagedList<MessageResponse>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = dbSet
            .OrderByDescending(x => x.MessageSent)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.Username && !x.RecipientDeleted),
            "Outbox" => query.Where(x => x.Sender.UserName == messageParams.Username && !x.SenderDeleted),
            _ => query.Where(x => x.Recipient.UserName == messageParams.Username && x.DateRead == null && !x.RecipientDeleted)
        };

        var messages = query.ProjectTo<MessageResponse>(_mapper.ConfigurationProvider);

        return await PagedList<MessageResponse>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageResponse>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var query = dbSet
            .Where(x =>
                (x.RecipientUsername == currentUsername && !x.RecipientDeleted && x.SenderUsername == recipientUsername) ||
                (x.SenderUsername == currentUsername && !x.SenderDeleted && x.RecipientUsername == recipientUsername))
            .OrderBy(x => x.MessageSent)
            .AsQueryable();

        var unreadMessages = query.Where(x => x.DateRead == null && x.RecipientUsername == currentUsername).ToList();

        if (unreadMessages.Count > 0)
        {
            unreadMessages.ForEach(x => x.DateRead = DateTime.UtcNow);
        }

        return await query.ProjectTo<MessageResponse>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public void AddGroup(Group group)
    {
        _dbContext.Groups.Add(group);
    }

    public void RemoveConnection(Connection connection)
    {
        _dbContext.Connections.Remove(connection);
    }
}