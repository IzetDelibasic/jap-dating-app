using MediatR;
using DatingApp.Infrastructure.Interfaces.IRepository;

public class GetTagsForPhotoHandler(IPhotoTagRepository photoTagRepository)
    : IRequestHandler<GetTagsForPhotoQuery, List<string>>
{
    public async Task<List<string>> Handle(GetTagsForPhotoQuery request, CancellationToken cancellationToken)
    {
        return await photoTagRepository.GetTagsForPhotoAsync(request.PhotoId);
    }
}