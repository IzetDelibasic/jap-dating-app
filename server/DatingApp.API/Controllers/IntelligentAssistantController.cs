using Microsoft.AspNetCore.Mvc;
using DatingApp.Services.Services;
using DatingApp.Common.DTO;
using DatingApp.Exceptions;
using DatingApp.Controllers;
using DatingApp.Infrastructure.Interfaces.IServices;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntelligentAssistantController(IIntelligentAssistantService intelligentAssistantService) : BaseApiController
    {
        [HttpPost("generate-message")]
        public async Task<IActionResult> GenerateMessage([FromBody] DatingMessageDto userMessageDto)
        {
            if (string.IsNullOrEmpty(userMessageDto.Interests) || string.IsNullOrEmpty(userMessageDto.LookingFor))
            {
                throw new BadRequestException("Interests and LookingFor properties must not be null or empty.");
            }

            var message = await intelligentAssistantService.GenerateIntroductionMessageAsync(userMessageDto.Interests, userMessageDto.LookingFor);
            return Ok(new { Message = message });
        }
    }
}