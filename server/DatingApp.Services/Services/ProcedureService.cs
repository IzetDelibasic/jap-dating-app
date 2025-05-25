using DatingApp.Common.DTO;
using DatingApp.Data;
using DatingApp.Infrastructure.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services
{
    public class ProcedureService(DatabaseContext databaseContext) : IProcedureService
    {
        public async Task<List<PhotoApprovalStatsDto>> GetPhotoApprovalStatsAsync()
        {
            return await databaseContext.PhotoApprovalStats
                .FromSqlRaw("EXEC GetPhotoApprovalStats")
                .ToListAsync();
        }

        public async Task<List<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync()
        {
            return await databaseContext.UsersWithoutMainPhoto
                .FromSqlRaw("EXEC GetUsersWithoutMainPhoto")
                .ToListAsync();
        }
    }
}