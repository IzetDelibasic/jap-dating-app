using DatingApp.Core.Entities;
using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository;

public class TagRepository(DatabaseContext dbContext)
    : BaseRepository<Tag>(dbContext), ITagRepository
{
    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await dbSet.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<bool> AddTagAsync(Tag tag)
    {
        dbSet.Add(tag);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteTagAsync(Tag tag)
    {
        dbSet.Remove(tag);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Photo>> GetPhotosByTagForUserAsync(string username, string tagName)
    {
        var photos = await dbContext.PhotoTags
            .Where(pt => pt.Tag != null
                && pt.Tag.Name == tagName
                && pt.Photo != null
                && pt.Photo.AppUser != null
                && pt.Photo.AppUser.UserName == username)
            .Select(pt => pt.Photo!)
            .ToListAsync();

        return photos ?? Enumerable.Empty<Photo>();
    }
}