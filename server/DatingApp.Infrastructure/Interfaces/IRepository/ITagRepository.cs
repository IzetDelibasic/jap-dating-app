using DatingApp.Core.Entities;
using DatingApp.Entities;

namespace DatingApp.Repository.Interfaces;

public interface ITagRepository : IBaseRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string name);
    Task<bool> AddTagAsync(Tag tag);
    Task<bool> DeleteTagAsync(Tag tag);
    Task<IEnumerable<Photo>> GetPhotosByTagForUserAsync(string username, string tagName);
}