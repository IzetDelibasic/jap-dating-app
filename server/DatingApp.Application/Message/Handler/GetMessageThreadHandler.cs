using MediatR;
using DatingApp.Repository.Interfaces;
using AutoMapper;
using DatingApp.Application.Contracts.Responses;

public class GetMessageThreadHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    IRequestHandler<GetMessageThreadQuery, IEnumerable<MessageResponse>>
{
    public async Task<IEnumerable<MessageResponse>> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
    {
        var messages = await unitOfWork.MessageRepository.GetMessageThread(request.CurrentUsername, request.RecipientUsername);
        return messages.Select(message => mapper.Map<MessageResponse>(message));
    }
}