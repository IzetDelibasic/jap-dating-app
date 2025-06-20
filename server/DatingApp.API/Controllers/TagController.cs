using DatingApp.Application.Contracts.Responses;
using DatingApp.Controllers;
using DatingApp.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tag")]
    public class TagController(IMediator mediator) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagResponse>>> GetAllTags()
        {
            var tags = await mediator.Send(new GetAllTagsQuery());
            return Ok(tags);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] TagResponse tagDto)
        {
            var result = await mediator.Send(new AddTagCommand { Tag = tagDto });
            if (!result)
                throw new BadRequestException("Tag already exists or could not be added.");
            return Ok("Tag added successfully.");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{tagName}")]
        public async Task<IActionResult> DeleteTag(string tagName)
        {
            var result = await mediator.Send(new DeleteTagCommand { TagName = tagName });
            if (!result)
                throw new NotFoundException("Tag not found or could not be deleted.");
            return Ok("Tag deleted successfully.");
        }

        [HttpGet("photos/{tagName}")]
        public async Task<IActionResult> GetPhotosByTagForUser(string tagName)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedException("User is not authenticated.");

            var photos = await mediator.Send(new GetPhotosByTagForUserQuery
            {
                Username = username,
                TagName = tagName
            });

            if (!photos.Any())
                throw new NotFoundException("No photos found for the given tag and user.");

            return Ok(photos);
        }
    }
}
