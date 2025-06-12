using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
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

    public async Task<IEnumerable<PhotoDto>> GetPhotosByUserId(int userId)
    {
        return await dbSet
            .IgnoreQueryFilters()
            .Where(x => x.AppUserId == userId)
            .Select(u => new PhotoDto
            {
                Id = u.Id,
                Url = u.Url,
                IsApproved = u.IsApproved,
                IsMain = u.IsMain,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
    {
        return await dbSet
            .IgnoreQueryFilters()
            .Where(x => x.IsApproved == false)
            .Select(u => new PhotoForApprovalDto
            {
                Id = u.Id,
                Username = u.AppUser.UserName,
                Url = u.Url,
                IsApproved = u.IsApproved
            })
            .ToListAsync();
    }
}