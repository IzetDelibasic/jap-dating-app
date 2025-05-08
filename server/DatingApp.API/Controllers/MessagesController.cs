using DatingApp.Entities.DTO;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Services.Interfaces;
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
            if (message == null) return BadRequest("Failed to save message");
            return Ok(message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await messagesService.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages);
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var messages = await messagesService.GetMessageThread(User.GetUsername(), username);
            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var result = await messagesService.DeleteMessage(User.GetUsername(), id);
            if (result) return Ok();
            return BadRequest("Problem deleting the message");
        }
    }
}