using DatingApp.Common.DTO;
using DatingApp.Core.Entities;

namespace DatingApp.Infrastructure.Interfaces.IServices;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync();
    Task<bool> AddTagAsync(TagDto tagDto);
    Task<bool> DeleteTagAsync(string tagName);
}
