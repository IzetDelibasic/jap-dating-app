using DatingApp.Entities.DTO;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Services.Interfaces;
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
        Response.AddPaginationHeader(users);
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto?>> GetUser(string username)
    {
        var user = await usersService.GetUser(username, User.GetUsername());
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var success = await usersService.UpdateUser(User.GetUsername(), memberUpdateDto);
        if (success) return NoContent();
        return BadRequest("Failed to update the user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var photoDto = await usersService.AddPhoto(User.GetUsername(), file);
        if (photoDto == null) return BadRequest("Problem adding photo");
        return CreatedAtAction(nameof(GetUser), new { username = User.GetUsername() }, photoDto);
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var success = await usersService.SetMainPhoto(User.GetUsername(), photoId);
        if (success) return NoContent();
        return BadRequest("Problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<IActionResult> DeletePhoto(int photoId)
    {
        var success = await usersService.DeletePhoto(User.GetUsername(), photoId);
        if (success) return Ok();
        return BadRequest("Problem deleting photo");
    }
}