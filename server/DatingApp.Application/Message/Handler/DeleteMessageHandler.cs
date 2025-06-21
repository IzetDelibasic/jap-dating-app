using MediatR;
using DatingApp.Repository.Interfaces;

public class DeleteMessageHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageCommand, bool>
{
    public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await unitOfWork.MessageRepository.GetByIdAsync(request.MessageId);

        if (message == null) return false;

        if (message.SenderUsername != request.Username && message.RecipientUsername != request.Username)
            throw new UnauthorizedAccessException();

        if (message.SenderUsername == request.Username) message.SenderDeleted = true;
        if (message.RecipientUsername == request.Username) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted)
        {
            unitOfWork.MessageRepository.Delete(message);
        }

        return await unitOfWork.Complete();
    }
}