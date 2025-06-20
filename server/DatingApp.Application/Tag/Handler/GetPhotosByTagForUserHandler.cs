using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Application.Contracts.Responses;

public class GetPhotosByTagForUserHandler(ITagRepository tagRepository) : IRequestHandler<GetPhotosByTagForUserQuery, IEnumerable<PhotoResponse>>
{
    public async Task<IEnumerable<PhotoResponse>> Handle(GetPhotosByTagForUserQuery request, CancellationToken cancellationToken)
    {
        var photos = await tagRepository.GetPhotosByTagForUserAsync(request.Username, request.TagName);

        return photos.Select(photo => new PhotoResponse
        {
            Id = photo.Id,
            Url = photo.Url,
            IsMain = photo.IsMain,
            IsApproved = photo.IsApproved
        });
    }
}