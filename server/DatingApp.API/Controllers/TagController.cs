using DatingApp.Common.DTO;
using DatingApp.Controllers;
using DatingApp.Core.Entities;
using DatingApp.Exceptions;
using DatingApp.Infrastructure.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tag")]
    public class TagController(ITagService tagService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllTags()
        {
            var tags = await tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] TagDto tagDto)
        {
            var result = await tagService.AddTagAsync(tagDto);
            if (!result)
            {
                throw new BadRequestException("Tag already exists or could not be added.");
            }
            return Ok("Tag added successfully.");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{tagName}")]
        public async Task<IActionResult> DeleteTag(string tagName)
        {
            var result = await tagService.DeleteTagAsync(tagName);
            if (!result)
            {
                throw new NotFoundException("Tag not found or could not be deleted.");
            }
            return Ok("Tag deleted successfully.");
        }

        [HttpGet("photos/by-tag/{tagName}")]
        public async Task<IActionResult> GetPhotosByTagForUser(string tagName)
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("User is not authenticated.");
            }

            var photos = await tagService.GetPhotosByTagForUserAsync(username, tagName);

            if (!photos.Any())
            {
                return NotFound("No photos found for the given tag and user.");
            }

            return Ok(photos);
        }
    }
}
