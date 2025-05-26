using DatingApp.Common.DTO;
using DatingApp.Core.Entities;
using DatingApp.Entities;
using DatingApp.Entities.DTO;

namespace DatingApp.Infrastructure.Interfaces.IServices;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync();
    Task<bool> AddTagAsync(TagDto tagDto);
    Task<bool> DeleteTagAsync(string tagName);
    Task<IEnumerable<PhotoDto>> GetPhotosByTagForUserAsync(string username, string tagName);
}
