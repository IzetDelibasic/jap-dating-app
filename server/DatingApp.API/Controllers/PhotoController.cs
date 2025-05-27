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
    public class PhotoController(IPhotoService photoService) : BaseApiController
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
