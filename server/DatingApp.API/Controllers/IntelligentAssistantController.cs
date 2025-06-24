using Microsoft.AspNetCore.Mvc;
using DatingApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using DatingApp.Application.Contracts.Requests;
using DatingApp.Exceptions;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    public class IntelligentAssistantController(IMediator mediator) : BaseApiController
    {
        [HttpPost("generate-message")]
        public async Task<IActionResult> GenerateMessage([FromBody] GenerateMessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Interests) || string.IsNullOrEmpty(request.LookingFor))
            {
                throw new BadRequestException("Interests and LookingFor properties must not be null or empty.");
            }

            var message = await mediator.Send(new GenerateMessageCommand
            {
                Interests = request.Interests,
                LookingFor = request.LookingFor
            });
            return Ok(new { Message = message });
        }
    }
}