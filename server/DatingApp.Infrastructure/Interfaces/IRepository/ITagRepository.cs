using DatingApp.Core.Entities;

namespace DatingApp.Repository.Interfaces;

public interface ITagRepository : IBaseRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string name);
    Task<bool> AddTagAsync(Tag tag);
    Task<bool> DeleteTagAsync(Tag tag);
}