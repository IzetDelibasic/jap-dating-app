using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Application.Contracts.Requests;

namespace DatingApp.Controllers
{
    [Authorize]
    [Route("api/messages")]
    [ApiController]
    public class MessagesController(IMediator mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageResponse>> CreateMessage(SendMessageRequest createMessageDto)
        {
            var message = await mediator.Send(new CreateMessageCommand
            {
                SenderUsername = User.GetUsername(),
                Request = createMessageDto
            });
            if (message == null)
            {
                throw new BadRequestException("Failed to save message.");
            }
            return Ok(message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageResponse>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await mediator.Send(new GetMessagesForUserQuery
            {
                Params = messageParams
            });
            if (messages == null)
            {
                throw new NotFoundException("No messages found for the user.");
            }
            Response.AddPaginationHeader(messages);
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageResponse>>> GetMessageThread(string username)
        {
            var messages = await mediator.Send(new GetMessageThreadQuery
            {
                CurrentUsername = User.GetUsername(),
                RecipientUsername = username
            });
            if (messages == null || !messages.Any())
            {
                throw new NotFoundException($"No message thread found with user '{username}'.");
            }
            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var result = await mediator.Send(new DeleteMessageCommand
            {
                Username = User.GetUsername(),
                MessageId = id
            });
            if (!result)
            {
                throw new BadRequestException("Problem deleting the message.");
            }
            return Ok();
        }
    }
}