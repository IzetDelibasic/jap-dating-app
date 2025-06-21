using MediatR;
using DatingApp.Repository.Interfaces;

public class DeleteTagHandler(ITagRepository tagRepository) : IRequestHandler<DeleteTagCommand, bool>
{
    public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByNameAsync(request.TagName);
        if (tag == null)
            return false;
        return await tagRepository.DeleteTagAsync(tag);
    }
}