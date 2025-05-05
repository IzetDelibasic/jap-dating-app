using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPhotoService photoService;
        public AdminController(UserManager<AppUser> _userManager, IUnitOfWork _unitOfWork, IPhotoService _photoService)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
            photoService = _photoService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await userManager.Users
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    x.Id,
                    Username = x.UserName,
                    Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
                }).ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("User not found");

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));
        }

        // 10. Implement the AdminController GetPhotosForApproval method

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            var photos = await unitOfWork.PhotoRepository.GetUnapprovedPhotos();

            return Ok(photos);
        }

        // 11. Add a method in the Admin Controller to Approve a photo
        // 14. Add the logic in the Admin controller approve photo method to check to see if the user has anyphotos that are set to main, if not then set the photo to main when approving.

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{id}")]
        public async Task<ActionResult> ApprovePhoto(int id)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoById(id);

            if (photo == null) return BadRequest("Can not get photo");

            photo.IsApproved = true;

            var user = await unitOfWork.UserRepository.GetUserByPhotoId(id);

            if (user == null) return BadRequest("Can not get user");

            if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

            await unitOfWork.Complete();

            return Ok();
        }

        // 12. Add a method in the Admin controller to reject a photo

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{id}")]
        public async Task<ActionResult> RejectPhoto(int id)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoById(id);

            if (photo == null) return BadRequest("Can not get photo");

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Result == "ok")
                {
                    unitOfWork.PhotoRepository.RemovePhoto(photo);
                }
            }

            await unitOfWork.Complete();

            return Ok();
        }
    }
}
