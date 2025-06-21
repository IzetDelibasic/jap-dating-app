using MediatR;
using Microsoft.AspNetCore.Identity;
using DatingApp.Entities;

public class GetUserRolesHandler(UserManager<AppUser> userManager) : IRequestHandler<GetUserRolesQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Username);
        return user != null ? await userManager.GetRolesAsync(user) : Enumerable.Empty<string>();
    }
}