using DatingApp.Entities;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services
{
    public class AdminService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService) : IAdminService
    {
        public async Task<IEnumerable<object>> GetUsersWithRoles()
        {
            return await userManager.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    x.Id,
                    Username = x.UserName,
                    Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
                }).ToListAsync();
        }

        public async Task<bool> EditRoles(string username, string roles)
        {
            if (string.IsNullOrEmpty(roles)) return false;

            var selectedRoles = roles.Split(",").ToArray();
            var user = await userManager.FindByNameAsync(username);

            if (user == null) return false;

            var userRoles = await userManager.GetRolesAsync(user);

            var addResult = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!addResult.Succeeded) return false;

            var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            return removeResult.Succeeded;
        }

        public async Task<IEnumerable<object>> GetPhotosForModeration()
        {
            return await unitOfWork.PhotoRepository.GetUnapprovedPhotos();
        }

        public async Task<bool> ApprovePhoto(int id)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoById(id);
            if (photo == null) return false;

            photo.IsApproved = true;

            var user = await unitOfWork.UserRepository.GetUserByPhotoId(id);
            if (user == null) return false;

            if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

            return await unitOfWork.Complete();
        }

        public async Task<bool> RejectPhoto(int id)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoById(id);
            if (photo == null) return false;

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Result == "ok")
                {
                    unitOfWork.PhotoRepository.Delete(photo);
                }
            }

            return await unitOfWork.Complete();
        }

        public async Task<IEnumerable<string>> GetUserRoles(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            return user != null ? await userManager.GetRolesAsync(user) : Enumerable.Empty<string>();
        }
    }
}