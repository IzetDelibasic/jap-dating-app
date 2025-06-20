using DatingApp.Application.Contracts.Requests;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterUserRequest registerDto)
        {
            var result = await mediator.Send(new RegisterUserCommand { Request = registerDto });
            if (result == null)
            {
                throw new BadRequestException("Failed to register user");
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginDto)
        {
            var result = await mediator.Send(new LoginUserQuery { Request = loginDto });
            if (result == null)
            {
                throw new UnauthorizedException("Invalid username or password");
            }
            return Ok(result);
        }
    }
}