using DatingApp.Common.DTO;

namespace DatingApp.Infrastructure.Interfaces.IServices;

public interface IProcedureService
{
    Task<List<PhotoApprovalStatsDto>> GetPhotoApprovalStatsAsync();
    Task<List<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync();
}
