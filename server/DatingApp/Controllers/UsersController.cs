using System;
using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(DataContext dc) : BaseApiController
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await dc.Users.ToListAsync();

        return users;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await dc.Users.FindAsync(id);

        if (user == null) return NotFound();

        return user;
    }
}
