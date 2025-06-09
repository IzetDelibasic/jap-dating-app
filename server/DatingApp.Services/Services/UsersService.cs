using AutoMapper;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services;

public class UsersService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper) : IUsersService
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

    public async Task<IEnumerable<object>> GetUsersWithRoles()
    {
        return await userManager.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .OrderBy(x => x.UserName)
            .Select(x => new
            {
                x.Id,
                Username = x.UserName,
                Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
            }).ToListAsync();
    }

    public async Task<bool> EditRoles(string username, string roles)
    {
        if (string.IsNullOrEmpty(roles)) return false;

        var selectedRoles = roles.Split(",").ToArray();
        var user = await userManager.FindByNameAsync(username);

        if (user == null) return false;

        var userRoles = await userManager.GetRolesAsync(user);

        var addResult = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!addResult.Succeeded) return false;

        var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        return removeResult.Succeeded;
    }

    public async Task<IEnumerable<string>> GetUserRoles(string username)
    {
        var user = await userManager.FindByNameAsync(username);
        return user != null ? await userManager.GetRolesAsync(user) : Enumerable.Empty<string>();
    }

    public async Task<bool> UpdateUser(string username, MemberUpdateDto memberUpdateDto)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        if (user == null) return false;

        mapper.Map(memberUpdateDto, user);
        return await unitOfWork.Complete();
    }

}