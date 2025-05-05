using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository;

public class PhotoRepository(DataContext db) : IPhotoRepository
{
    // 9. Add a PhotoRepository that supports the following methods
    public async Task<Photo?> GetPhotoById(int id)
    {
        return await db.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
    {
        return await db.Photos.IgnoreQueryFilters().Where(x => x.IsApproved == false)
            .Select(u => new PhotoForApprovalDto
            {
                Id = u.Id,
                Username = u.AppUser.UserName,
                Url = u.Url,
                IsApproved = u.IsApproved
            }).ToListAsync();
    }

    public void RemovePhoto(Photo photo)
    {
        db.Photos.Remove(photo);
    }
}
