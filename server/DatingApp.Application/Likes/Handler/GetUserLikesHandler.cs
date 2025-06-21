using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Helpers;
using DatingApp.Application.Contracts.Responses;

public class GetUserLikesHandler(IUnitOfWork unitOfWork) : 
    IRequestHandler<GetUserLikesQuery, PagedList<MemberResponse>>
{
    public async Task<PagedList<MemberResponse>> Handle(GetUserLikesQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.LikesRepository.GetUserLikes(request.LikesParams);
    }
}