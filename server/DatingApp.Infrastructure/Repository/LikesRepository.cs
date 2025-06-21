using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository;

public class LikesRepository(DatabaseContext dbContext, IMapper mapper) : BaseRepository<UserLike>(dbContext), ILikesRepository
{
    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
        return await dbSet
            .Where(x => x.SourceUserId == currentUserId)
            .Select(x => x.TargetUserId)
            .ToListAsync();
    }

    public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await dbSet.FindAsync(sourceUserId, targetUserId);
    }

    public async Task<PagedList<MemberResponse>> GetUserLikes(LikesParams likesParams)
    {
        var likes = dbSet.AsQueryable();
        IQueryable<MemberResponse> query;

        switch (likesParams.Predicate)
        {
            case "liked":
                query = likes
                    .Where(x => x.SourceUserId == likesParams.UserId)
                    .Select(x => x.TargetUser)
                    .ProjectTo<MemberResponse>(mapper.ConfigurationProvider);
                break;
            case "likedBy":
                query = likes
                    .Where(x => x.TargetUserId == likesParams.UserId)
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberResponse>(mapper.ConfigurationProvider);
                break;
            default:
                var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);

                query = likes
                    .Where(x => x.TargetUserId == likesParams.UserId && likeIds.Contains(x.SourceUserId))
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberResponse>(mapper.ConfigurationProvider);
                break;
        }

        return await PagedList<MemberResponse>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
    }
}