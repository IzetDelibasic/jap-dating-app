using DatingApp.Services.Interfaces;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Infrastructure.Interfaces.IServices;

namespace DatingApp.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController(IAdminService adminService, IProcedureService procedureService) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await adminService.GetUsersWithRoles();
            if (users == null)
            {
                throw new NotFoundException("No users with roles found.");
            }
            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            var result = await adminService.EditRoles(username, roles);
            if (!result)
            {
                throw new BadRequestException($"Failed to update roles for user '{username}'.");
            }
            return Ok(await adminService.GetUserRoles(username));
        }

        [Authorize(Policy = "ModerateRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPendingPhotos()
        {
            var photos = await adminService.GetPhotosForModeration();
            if (photos == null)
            {
                throw new NotFoundException("No photos to moderate found.");
            }
            return Ok(photos);
        }

        [Authorize(Policy = "ModerateRole")]
        [HttpPost("approve-photo/{id}")]
        public async Task<ActionResult> ApprovePhotoById(int id)
        {
            var result = await adminService.ApprovePhoto(id);
            if (!result)
            {
                throw new BadRequestException($"Failed to approve photo with ID {id}.");
            }
            return Ok();
        }

        [Authorize(Policy = "ModerateRole")]
        [HttpPost("reject-photo/{id}")]
        public async Task<ActionResult> RejectPhotoById(int id)
        {
            var result = await adminService.RejectPhoto(id);
            if (!result)
            {
                throw new BadRequestException($"Failed to reject photo with ID {id}.");
            }
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("photo-approval-stats")]
        public async Task<IActionResult> GetPhotoApprovalStats()
        {
            var stats = await procedureService.GetPhotoApprovalStatsAsync();
            return Ok(stats);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("without-main-photo")]
        public async Task<IActionResult> GetUsersWithoutMainPhoto()
        {
            var users = await procedureService.GetUsersWithoutMainPhotoAsync();
            return Ok(users);
        }
    }
}