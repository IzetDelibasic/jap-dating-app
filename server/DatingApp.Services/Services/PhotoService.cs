using AutoMapper;
using DatingApp.Core.Entities;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Infrastructure.Interfaces.IServices;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Services.Services;

public class PhotoService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService) : IPhotoService
{
    public async Task<PhotoDto?> AddPhoto(string username, IFormFile file, List<string> tagNames)
    {
        if (tagNames == null || !tagNames.Any())
        {
            throw new Exception("Tags cannot be null or empty.");
        }

        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (user == null) return null;

        var result = await cloudinaryService.AddPhotoAsync(file);
        if (result.Error != null) throw new Exception(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            AppUserId = user.Id,
        };

        unitOfWork.PhotoRepository.Add(photo);

        foreach (var tagName in tagNames)
        {
            var tag = await unitOfWork.TagRepository.GetByNameAsync(tagName);
            if (tag == null)
            {
                throw new Exception($"Tag '{tagName}' does not exist.");
            }

            unitOfWork.PhotoTagRepository.Add(new PhotoTag { Photo = photo, Tag = tag });
        }

        user.Photos.Add(photo);
        if (await unitOfWork.Complete())
        {
            return mapper.Map<PhotoDto>(photo);
        }

        throw new Exception("Problem adding photo with tags");
    }

    public async Task<IEnumerable<object>> GetPhotosForModeration()
    {
        return await unitOfWork.PhotoRepository.GetUnapprovedPhotos();
    }

    public async Task<bool> ApprovePhoto(int id)
    {
        var photo = await unitOfWork.PhotoRepository.GetPhotoById(id);
        if (photo == null) return false;

        photo.IsApproved = true;

        var user = await unitOfWork.UserRepository.GetUserByPhotoId(id);
        if (user == null) return false;

        if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

        return await unitOfWork.Complete();
    }

    public async Task<bool> RejectPhoto(int id)
    {
        var photo = await unitOfWork.PhotoRepository.GetPhotoById(id);
        if (photo == null) return false;

        if (photo.PublicId != null)
        {
            var result = await cloudinaryService.DeletePhotoAsync(photo.PublicId);
            if (result.Result == "ok")
            {
                unitOfWork.PhotoRepository.Delete(photo);
            }
        }

        return await unitOfWork.Complete();
    }

    public async Task<bool> SetMainPhoto(string username, int photoId)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (user == null) return false;

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return false;

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        return await unitOfWork.Complete();
    }

    public async Task<bool> DeletePhoto(string username, int photoId)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (user == null) return false;

        var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);
        if (photo == null || photo.IsMain) return false;

        if (photo.PublicId != null)
        {
            var result = await cloudinaryService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) throw new Exception(result.Error.Message);
        }

        user.Photos.Remove(photo);
        return await unitOfWork.Complete();
    }

    public async Task<IEnumerable<PhotoDto>> GetPhotosByTagAsync(int tagId)
    {
        var photos = await unitOfWork.PhotoTagRepository.GetPhotosByTagIdAsync(tagId);

        return photos.Select(photo => new PhotoDto
        {
            Id = photo.Id,
            Url = photo.Url,
            IsMain = photo.IsMain,
            IsApproved = photo.IsApproved
        });
    }

    public async Task<List<string>> GetTagsForPhotoAsync(int photoId)
    {
        return await unitOfWork.PhotoTagRepository.GetTagsForPhotoAsync(photoId);
    }
}
