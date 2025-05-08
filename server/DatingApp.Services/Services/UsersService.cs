using AutoMapper;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Services;

public class UsersService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService) : IUsersService
{
    public async Task<PagedList<MemberDto>> GetUsers(UserParams userParams, string currentUsername)
    {
        userParams.CurrentUsername = currentUsername;
        return await unitOfWork.UserRepository.GetMembersAsync(userParams);
    }

    public async Task<MemberDto?> GetUser(string username, string currentUsername)
    {
        return await unitOfWork.UserRepository.GetMemberAsync(username, isCurrentUser: username == currentUsername);
    }

    public async Task<bool> UpdateUser(string username, MemberUpdateDto memberUpdateDto)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (user == null) return false;

        mapper.Map(memberUpdateDto, user);
        return await unitOfWork.Complete();
    }

    public async Task<PhotoDto?> AddPhoto(string username, IFormFile file)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (user == null) return null;

        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null) throw new Exception(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };

        user.Photos.Add(photo);
        if (await unitOfWork.Complete())
        {
            return mapper.Map<PhotoDto>(photo);
        }

        throw new Exception("Problem adding photo");
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
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) throw new Exception(result.Error.Message);
        }

        user.Photos.Remove(photo);
        return await unitOfWork.Complete();
    }
}