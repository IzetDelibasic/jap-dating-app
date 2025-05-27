using DatingApp.Core.Entities;
using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Infrastructure.Interfaces.IRepository;
using DatingApp.Repository;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Repository;

public class PhotoTagRepository : BaseRepository<PhotoTag>, IPhotoTagRepository
{
    public PhotoTagRepository(DatabaseContext context) : base(context) { }

    public async Task<IEnumerable<Photo>> GetPhotosByTagIdAsync(int tagId)
    {
        return await dbSet
            .Where(pt => pt.TagId == tagId)
            .Select(pt => pt.Photo!)
            .ToListAsync();
    }

    public async Task<List<string>> GetTagsForPhotoAsync(int photoId)
    {
        return await dbSet
            .Where(pt => pt.PhotoId == photoId)
            .Select(pt => pt.Tag!.Name)
            .ToListAsync();
    }
}

