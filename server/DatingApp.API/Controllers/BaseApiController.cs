using DatingApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected string GetUsername()
    {
        return User?.Identity?.Name ?? string.Empty;
    }

    protected int GetUserId()
    {
        return int.Parse(User.FindFirst("id")?.Value ?? "0");
    }
}
