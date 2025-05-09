using DatingApp.Entities.DTO;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Services.Interfaces;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Authorize]
    [Route("api/messages")]
    [ApiController]
    public class MessagesController(IMessagesService messagesService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var message = await messagesService.CreateMessage(User.GetUsername(), createMessageDto);
            if (message == null)
            {
                throw new BadRequestException("Failed to save message.");
            }
            return Ok(message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await messagesService.GetMessagesForUser(messageParams);
            if (messages == null)
            {
                throw new NotFoundException("No messages found for the user.");
            }
            Response.AddPaginationHeader(messages);
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var messages = await messagesService.GetMessageThread(User.GetUsername(), username);
            if (messages == null || !messages.Any())
            {
                throw new NotFoundException($"No message thread found with user '{username}'.");
            }
            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var result = await messagesService.DeleteMessage(User.GetUsername(), id);
            if (!result)
            {
                throw new BadRequestException("Problem deleting the message.");
            }
            return Ok();
        }
    }
}