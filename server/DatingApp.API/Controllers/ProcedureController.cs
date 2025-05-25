using DatingApp.Controllers;
using DatingApp.Infrastructure.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/procedure")]
    public class ProcedureController(IProcedureService procedureService) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("photo-approval-stats")]
        public async Task<IActionResult> GetPhotoApprovalStats()
        {
            var stats = await procedureService.GetPhotoApprovalStatsAsync();
            return Ok(stats);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-without-main-photo")]
        public async Task<IActionResult> GetUsersWithoutMainPhoto()
        {
            var users = await procedureService.GetUsersWithoutMainPhotoAsync();
            return Ok(users);
        }
    }
}