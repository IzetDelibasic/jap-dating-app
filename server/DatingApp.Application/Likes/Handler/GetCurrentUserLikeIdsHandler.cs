using MediatR;
using DatingApp.Repository.Interfaces;

public class GetCurrentUserLikeIdsHandler(IUnitOfWork unitOfWork) : 
    IRequestHandler<GetCurrentUserLikeIdsQuery, IEnumerable<int>>
{
    public async Task<IEnumerable<int>> Handle(GetCurrentUserLikeIdsQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.LikesRepository.GetCurrentUserLikeIds(request.UserId);
    }
}