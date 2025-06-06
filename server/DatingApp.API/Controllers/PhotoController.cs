using DatingApp.Common.DTO;
using DatingApp.Controllers;
using DatingApp.Entities.DTO;
using DatingApp.Exceptions;
using DatingApp.Extensions;
using DatingApp.Infrastructure.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/photo")]
    public class PhotoController(IPhotoService photoService, IProcedureService procedureService) : BaseApiController
    {
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm] PhotoUploadDto photoUploadDto)
        {
            if (photoUploadDto.Tags == null || !photoUploadDto.Tags.Any())
            {
                throw new BadRequestException("Tags cannot be null or empty.");
            }

            var photoDto = await photoService.AddPhoto(User.GetUsername(), photoUploadDto.File, photoUploadDto.Tags);
            if (photoDto == null)
            {
                throw new BadRequestException("Problem adding photo.");
            }

            return CreatedAtAction("GetUser", "Users", new { username = User.GetUsername() }, photoDto);
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var success = await photoService.SetMainPhoto(User.GetUsername(), photoId);
            if (!success)
            {
                throw new BadRequestException("Problem setting main photo.");
            }
            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var success = await photoService.DeletePhoto(User.GetUsername(), photoId);
            if (!success)
            {
                throw new BadRequestException("Problem deleting photo.");
            }
            return Ok();
        }

        [Authorize(Policy = "ModerateRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPendingPhotos()
        {
            var photos = await photoService.GetPhotosForModeration();
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
            var result = await photoService.ApprovePhoto(id);
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
            var result = await photoService.RejectPhoto(id);
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

        [HttpGet("by-tag/{tagId}")]
        public async Task<ActionResult<IEnumerable<PhotoDto>>> GetPhotosByTag(int tagId)
        {
            var photos = await photoService.GetPhotosByTagAsync(tagId);
            if (!photos.Any())
            {
                throw new NotFoundException("No photos found for the specified tag.");
            }
            return Ok(photos);
        }

        [HttpGet("{photoId}/tags")]
        public async Task<ActionResult<List<string>>> GetTagsForPhoto(int photoId)
        {
            var tags = await photoService.GetTagsForPhotoAsync(photoId);
            if (tags == null || !tags.Any())
            {
                tags = new List<string>();
                //throw new NotFoundException("No tags found for this photo.");
            }

            return Ok(tags);
        }
    }
}
