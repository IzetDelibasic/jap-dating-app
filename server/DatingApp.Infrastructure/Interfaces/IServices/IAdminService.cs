namespace DatingApp.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<object>> GetUsersWithRoles();
        Task<bool> EditRoles(string username, string roles);
        Task<IEnumerable<object>> GetPhotosForModeration();
        Task<bool> ApprovePhoto(int id);
        Task<bool> RejectPhoto(int id);
        Task<IEnumerable<string>> GetUserRoles(string username);
    }
}