using DatingApp.Common.DTO;
using DatingApp.Core.Entities;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
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

    public async Task<IEnumerable<PhotoDto>> GetPhotosByTagForUserAsync(string username, string tagName)
    {
        var photos = await tagRepository.GetPhotosByTagForUserAsync(username, tagName);

        return photos.Select(photo => new PhotoDto
        {
            Id = photo.Id,
            Url = photo.Url,
            IsMain = photo.IsMain,
            IsApproved = photo.IsApproved
        });
    }
}
