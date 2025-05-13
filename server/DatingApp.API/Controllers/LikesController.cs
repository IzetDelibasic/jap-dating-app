using DatingApp.Entities.DTO;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Services.Interfaces;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikesController(ILikesService likesService) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var result = await likesService.ToggleLike(User.GetUserId(), targetUserId);
            if (!result)
            {
                throw new BadRequestException("Failed to update like.");
            }
            return Ok();
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            var likeIds = await likesService.GetCurrentUserLikeIds(User.GetUserId());
            return Ok(likeIds);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await likesService.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}