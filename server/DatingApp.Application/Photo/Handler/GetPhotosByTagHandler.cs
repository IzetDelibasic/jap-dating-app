using MediatR;
using DatingApp.Infrastructure.Interfaces.IRepository;
using DatingApp.Application.Contracts.Responses;

public class GetPhotosByTagHandler(IPhotoTagRepository photoTagRepository)
    : IRequestHandler<GetPhotosByTagQuery, IEnumerable<PhotoResponse>>
{
    public async Task<IEnumerable<PhotoResponse>> Handle(GetPhotosByTagQuery request, CancellationToken cancellationToken)
    {
        var photos = await photoTagRepository.GetPhotosByTagIdAsync(request.TagId);

        return photos.Select(photo => new PhotoResponse
        {
            Id = photo.Id,
            Url = photo.Url,
            IsMain = photo.IsMain,
            IsApproved = photo.IsApproved
        });
    }
}