using DatingApp.Application.Contracts.Requests;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Controllers;
using DatingApp.Exceptions;
using DatingApp.Extensions;
using DatingApp.Infrastructure.Interfaces.IServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    public class PhotoController(IMediator mediator, IProcedureService procedureService) : BaseApiController
    {
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoResponse>> AddPhoto([FromForm] PhotoUploadRequest photoUploadDto)
        {
            if (photoUploadDto.Tags == null || !photoUploadDto.Tags.Any())
                throw new BadRequestException("Tags cannot be null or empty.");

            var photoDto = await mediator.Send(new AddPhotoCommand
            {
                Username = User.GetUsername(),
                File = photoUploadDto.File,
                Tags = photoUploadDto.Tags
            });

            if (photoDto == null)
                throw new BadRequestException("Problem adding photo.");

            return CreatedAtAction("GetUser", "Users", new { username = User.GetUsername() }, photoDto);
        }

        [HttpGet("get-photos-by-user/{id:int}")]
        public async Task<ActionResult<IEnumerable<PhotoResponse>>> GetPhotosByUserId(int id)
        {
            var photos = await mediator.Send(new GetPhotosByUserIdQuery { UserId = id });
            if (photos == null || !photos.Any())
                throw new NotFoundException("No photos found.");
            return Ok(photos);
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var success = await mediator.Send(new SetMainPhotoCommand
            {
                Username = User.GetUsername(),
                PhotoId = photoId
            });
            if (!success)
                throw new BadRequestException("Problem setting main photo.");
            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var success = await mediator.Send(new DeletePhotoCommand
            {
                Username = User.GetUsername(),
                PhotoId = photoId
            });
            if (!success)
                throw new BadRequestException("Problem deleting photo.");
            return Ok();
        }

        [Authorize(Policy = "ModerateRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPendingPhotos()
        {
            var photos = await mediator.Send(new GetPhotosForModerationQuery());
            if (photos == null)
                throw new NotFoundException("No photos to moderate found.");
            return Ok(photos);
        }

        [Authorize(Policy = "ModerateRole")]
        [HttpPost("approve-photo/{id}")]
        public async Task<ActionResult> ApprovePhotoById(int id)
        {
            var result = await mediator.Send(new ApprovePhotoCommand { PhotoId = id });
            if (!result)
                throw new BadRequestException($"Failed to approve photo with ID {id}.");
            return Ok();
        }

        [Authorize(Policy = "ModerateRole")]
        [HttpPost("reject-photo/{id}")]
        public async Task<ActionResult> RejectPhotoById(int id)
        {
            var result = await mediator.Send(new RejectPhotoCommand { PhotoId = id });
            if (!result)
                throw new BadRequestException($"Failed to reject photo with ID {id}.");
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
        public async Task<ActionResult<IEnumerable<PhotoResponse>>> GetPhotosByTag(int tagId)
        {
            var photos = await mediator.Send(new GetPhotosByTagQuery { TagId = tagId });
            if (!photos.Any())
                throw new NotFoundException("No photos found for the specified tag.");
            return Ok(photos);
        }

        [HttpGet("{photoId}/tags")]
        public async Task<ActionResult<List<string>>> GetTagsForPhoto(int photoId)
        {
            var tags = await mediator.Send(new GetTagsForPhotoQuery { PhotoId = photoId });
            if (tags == null || !tags.Any())
                tags = new List<string>();
            return Ok(tags);
        }
    }
}
