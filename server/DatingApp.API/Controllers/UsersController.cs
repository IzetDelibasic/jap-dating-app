using DatingApp.Entities.DTO;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Services.Interfaces;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UsersController(IUsersService usersService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        var users = await usersService.GetUsers(userParams, User.GetUsername());
        if (users == null || !users.Any())
        {
            throw new NotFoundException("No users found.");
        }
        Response.AddPaginationHeader(users);
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto?>> GetUser(string username)
    {
        var user = await usersService.GetUser(username, User.GetUsername());
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }
        return Ok(user);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await usersService.GetUsersWithRoles();
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
        var result = await usersService.EditRoles(username, roles);
        if (!result)
        {
            throw new BadRequestException($"Failed to update roles for user '{username}'.");
        }
        return Ok(await usersService.GetUserRoles(username));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var success = await usersService.UpdateUser(User.GetUsername(), memberUpdateDto);
        if (!success)
        {
            throw new BadRequestException("Failed to update the user.");
        }
        return NoContent();
    }

}