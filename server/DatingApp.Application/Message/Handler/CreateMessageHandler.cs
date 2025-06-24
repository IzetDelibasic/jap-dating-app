using MediatR;
using DatingApp.Repository.Interfaces;
using AutoMapper;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;
using DatingApp.Exceptions;

public class CreateMessageHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    IRequestHandler<CreateMessageCommand, MessageResponse?>
{
    public async Task<MessageResponse?> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        if (request.SenderUsername == request.Request.RecipientUsername.ToLower())
            throw new BadRequestException("You cannot message yourself");

        var sender = await unitOfWork.UserRepository.GetUserByUsernameAsync(request.SenderUsername);
        var recipient = await unitOfWork.UserRepository.GetUserByUsernameAsync(request.Request.RecipientUsername);

        if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null)
            return null;

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = request.Request.Content,
        };

        unitOfWork.MessageRepository.Add(message);

        if (await unitOfWork.Complete())
            return mapper.Map<MessageResponse>(message);

        return null;
    }
}