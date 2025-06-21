using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Application.Contracts.Responses;

public class GetAllTagsHandler(ITagRepository tagRepository) : IRequestHandler<GetAllTagsQuery, IEnumerable<TagResponse>>
{
    public async Task<IEnumerable<TagResponse>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await tagRepository.GetAllAsync();
        return tags.Select(tag => new TagResponse { Name = tag.Name });
    }
}