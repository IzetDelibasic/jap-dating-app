using DatingApp.Application.Contracts.Responses;
using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository;

public class PhotoRepository(DatabaseContext dbContext) : BaseRepository<Photo>(dbContext), IPhotoRepository
{
    public async Task<Photo?> GetPhotoById(int id)
    {
        return await dbSet
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PhotoResponse>> GetPhotosByUserId(int userId)
    {
        return await dbSet
            .IgnoreQueryFilters()
            .Where(x => x.AppUserId == userId)
            .Select(u => new PhotoResponse
            {
                Id = u.Id,
                Url = u.Url,
                IsApproved = u.IsApproved,
                IsMain = u.IsMain,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PhotoForApprovalResponse>> GetUnapprovedPhotos()
    {
        return await dbSet
            .IgnoreQueryFilters()
            .Where(x => x.IsApproved == false)
            .Select(u => new PhotoForApprovalResponse
            {
                Id = u.Id,
                Username = u.AppUser.UserName,
                Url = u.Url,
                IsApproved = u.IsApproved
            })
            .ToListAsync();
    }
}