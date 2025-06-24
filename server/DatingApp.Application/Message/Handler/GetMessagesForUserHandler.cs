using MediatR;
using DatingApp.Repository.Interfaces;
using AutoMapper;
using DatingApp.Helpers;
using DatingApp.Application.Contracts.Responses;

public class GetMessagesForUserHandler(IUnitOfWork unitOfWork, IMapper mapper) : 
    IRequestHandler<GetMessagesForUserQuery, PagedList<MessageResponse>>
{
    public async Task<PagedList<MessageResponse>> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
    {
        var pagedList = await unitOfWork.MessageRepository.GetMessagesForUser(request.Params);

        var mapped = pagedList.Select(message => mapper.Map<MessageResponse>(message)).ToList();

        return new PagedList<MessageResponse>(
            mapped,
            pagedList.TotalCount,
            pagedList.CurrentPage,
            pagedList.PageSize
        );
    }
}