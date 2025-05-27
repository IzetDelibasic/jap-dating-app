using DatingApp.Core.Entities;
using DatingApp.Entities;
using DatingApp.Repository.Interfaces;

namespace DatingApp.Infrastructure.Interfaces.IRepository;

public interface IPhotoTagRepository : IBaseRepository<PhotoTag>
{
    Task<IEnumerable<Photo>> GetPhotosByTagIdAsync(int tagId);
    Task<List<string>> GetTagsForPhotoAsync(int photoId);
}
