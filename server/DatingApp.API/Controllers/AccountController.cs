using DatingApp.Entities.DTO;
using DatingApp.Exceptions;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(IAccountService accountService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var result = await accountService.Register(registerDto);
            if (result == null)
            {
                throw new BadRequestException("Failed to register user");
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await accountService.Login(loginDto);
            if (result == null)
            {
                throw new UnauthorizedException("Invalid username or password");
            }
            return Ok(result);
        }
    }
}