using MediatR;
using Microsoft.AspNetCore.Identity;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;
using DatingApp.Common.Contracts.Response;

public class GetUsersWithRolesHandler(UserManager<AppUser> userManager)
    : IRequestHandler<GetUsersWithRolesQuery, IEnumerable<UserWithRolesResponse>>
{
    public async Task<IEnumerable<UserWithRolesResponse>> Handle(GetUsersWithRolesQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users
            .AsNoTracking()
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);

        var result = new List<UserWithRolesResponse>();
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            result.Add(new UserWithRolesResponse
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Roles = roles.ToList()
            });
        }

        return result;
    }
}