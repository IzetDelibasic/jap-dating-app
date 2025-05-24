using DatingApp.Entities.DTO;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Infrastructure.Interfaces.IServices;

public interface IPhotoService
{
    Task<PhotoDto?> AddPhoto(string username, IFormFile file, List<string> tagIds);
    Task<bool> SetMainPhoto(string username, int photoId);
    Task<bool> DeletePhoto(string username, int photoId);
    Task<IEnumerable<PhotoDto>> GetPhotosByTagAsync(int tagId);
}
