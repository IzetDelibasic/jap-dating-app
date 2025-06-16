using DatingApp.Entities.DTO;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Infrastructure.Interfaces.IServices;

public interface IPhotoService
{
    Task<PhotoDto?> AddPhoto(string username, IFormFile file, List<string> tagIds);
    Task<IEnumerable<PhotoDto>> GetPhotosByUserId(int id);
    Task<IEnumerable<object>> GetPhotosForModeration();
    Task<bool> ApprovePhoto(int id);
    Task<bool> RejectPhoto(int id);
    Task<bool> SetMainPhoto(string username, int photoId);
    Task<bool> DeletePhoto(string username, int photoId);
    Task<IEnumerable<PhotoDto>> GetPhotosByTagAsync(int tagId);
    Task<List<string>> GetTagsForPhotoAsync(int photoId);
}
