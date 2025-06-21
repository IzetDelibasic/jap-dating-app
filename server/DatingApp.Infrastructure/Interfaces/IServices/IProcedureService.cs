using DatingApp.Application.Contracts.Responses;

namespace DatingApp.Infrastructure.Interfaces.IServices;

public interface IProcedureService
{
    Task<List<PhotoApprovalStatsResponse>> GetPhotoApprovalStatsAsync();
    Task<List<UserWithoutMainPhotoResponse>> GetUsersWithoutMainPhotoAsync();
}
