using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;

namespace DatingApp.Repository.Interfaces;

public interface IPhotoRepository : IBaseRepository<Photo>
{
    Task<Photo?> GetPhotoById(int id);
    Task<IEnumerable<PhotoResponse>> GetPhotosByUserId(int userId);
    Task<IEnumerable<PhotoForApprovalResponse>> GetUnapprovedPhotos();
}