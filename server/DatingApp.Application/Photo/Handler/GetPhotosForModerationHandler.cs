using MediatR;
using DatingApp.Repository.Interfaces;

public class GetPhotosForModerationHandler(IPhotoRepository photoRepository)
    : IRequestHandler<GetPhotosForModerationQuery, IEnumerable<object>>
{
    public async Task<IEnumerable<object>> Handle(GetPhotosForModerationQuery request, CancellationToken cancellationToken)
    {
        return await photoRepository.GetUnapprovedPhotos();
    }
}