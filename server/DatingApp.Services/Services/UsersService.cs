using AutoMapper;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;

namespace DatingApp.Services;

public class UsersService(IUnitOfWork unitOfWork, IMapper mapper) : IUsersService
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

}