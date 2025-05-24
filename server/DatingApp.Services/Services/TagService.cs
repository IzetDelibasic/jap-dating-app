using DatingApp.Common.DTO;
using DatingApp.Core.Entities;
using DatingApp.Infrastructure.Interfaces.IServices;
using DatingApp.Repository.Interfaces;

namespace DatingApp.Services.Services;

public class TagService(ITagRepository tagRepository) : ITagService
{
    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        return await tagRepository.GetAllAsync();
    }

    public async Task<bool> AddTagAsync(TagDto tagDto)
    {
        var existingTag = await tagRepository.GetByNameAsync(tagDto.Name);
        if (existingTag != null)
        {
            return false;
        }

        var tag = new Tag { Name = tagDto.Name };
        return await tagRepository.AddTagAsync(tag);
    }

    public async Task<bool> DeleteTagAsync(string tagName)
    {
        var tag = await tagRepository.GetByNameAsync(tagName);
        if (tag == null)
        {
            return false;
        }
        return await tagRepository.DeleteTagAsync(tag);
    }
}
