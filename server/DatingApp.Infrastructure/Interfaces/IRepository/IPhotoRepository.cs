using DatingApp.Entities;
using DatingApp.Entities.DTO;

namespace DatingApp.Repository.Interfaces;

public interface IPhotoRepository : IBaseRepository<Photo>
{
    Task<Photo?> GetPhotoById(int id);
    Task<IEnumerable<PhotoDto>> GetPhotosByUserId(int userId);
    Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
}