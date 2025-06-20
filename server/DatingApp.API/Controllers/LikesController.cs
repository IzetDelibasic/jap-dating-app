using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DatingApp.Application.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.Controllers
{
    [Authorize]
    [ApiController]
    public class LikesController(IMediator mediator) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var result = await mediator.Send(new ToggleLikeCommand
            {
                SourceUserId = User.GetUserId(),
                TargetUserId = targetUserId
            });
            if (!result)
            {
                throw new BadRequestException("Failed to update like.");
            }
            return Ok();
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            var likeIds = await mediator.Send(new GetCurrentUserLikeIdsQuery
            {
                UserId = User.GetUserId()
            });
            return Ok(likeIds);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberResponse>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await mediator.Send(new GetUserLikesQuery
            {
                LikesParams = likesParams
            });
            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}