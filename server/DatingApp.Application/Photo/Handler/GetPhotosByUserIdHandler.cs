using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Application.Contracts.Responses;

public class GetPhotosByUserIdHandler(IPhotoRepository photoRepository)
    : IRequestHandler<GetPhotosByUserIdQuery, IEnumerable<PhotoResponse>>
{
    public async Task<IEnumerable<PhotoResponse>> Handle(GetPhotosByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await photoRepository.GetPhotosByUserId(request.UserId);
    }
}