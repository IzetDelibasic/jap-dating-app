using DatingApp.Extensions;
using DatingApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Application.Contracts.Requests;
using DatingApp.Application.Contracts.Responses;
using MediatR;

namespace DatingApp.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UsersController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetUsers([FromQuery] UserParams userParams)
    {
        var users = await mediator.Send(new GetUsersQuery
        {
            UserParams = userParams,
            CurrentUsername = User.GetUsername()
        });
        Response.AddPaginationHeader(users);
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberResponse?>> GetUser(string username)
    {
        var user = await mediator.Send(new GetUserByUsernameQuery
        {
            Username = username,
            CurrentUsername = User.GetUsername()
        });
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateMemberRequest memberUpdateDto)
    {
        var success = await mediator.Send(new UpdateUserCommand
        {
            Username = User.GetUsername(),
            Request = memberUpdateDto
        });
        if (!success)
        {
            return BadRequest("Failed to update the user.");
        }
        return NoContent();
    }
}