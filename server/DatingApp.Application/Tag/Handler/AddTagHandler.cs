using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Core.Entities;

public class AddTagHandler(ITagRepository tagRepository) : IRequestHandler<AddTagCommand, bool>
{
    public async Task<bool> Handle(AddTagCommand request, CancellationToken cancellationToken)
    {
        var existingTag = await tagRepository.GetByNameAsync(request.Tag.Name);
        if (existingTag != null)
            return false;

        var tag = new Tag { Name = request.Tag.Name };
        return await tagRepository.AddTagAsync(tag);
    }
}