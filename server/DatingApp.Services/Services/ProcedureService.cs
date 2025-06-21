using DatingApp.Application.Contracts.Responses;
using DatingApp.Data;
using DatingApp.Infrastructure.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services
{
    public class ProcedureService(DatabaseContext databaseContext) : IProcedureService
    {
        public async Task<List<PhotoApprovalStatsResponse>> GetPhotoApprovalStatsAsync()
        {
            return await databaseContext.PhotoApprovalStats
                .FromSqlRaw("EXEC GetPhotoApprovalStats")
                .ToListAsync();
        }

        public async Task<List<UserWithoutMainPhotoResponse>> GetUsersWithoutMainPhotoAsync()
        {
            return await databaseContext.UsersWithoutMainPhoto
                .FromSqlRaw("EXEC GetUsersWithoutMainPhoto")
                .ToListAsync();
        }
    }
}