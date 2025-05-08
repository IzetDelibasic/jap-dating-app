using DatingApp.Entities;
using DatingApp.Entities.DTO;

namespace DatingApp.Repository.Interfaces;

public interface IPhotoRepository
{
    // 9. Add a PhotoRepository that supports the following methods
    Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
    Task<Photo?> GetPhotoById(int id);
    void RemovePhoto(Photo photo);
}
