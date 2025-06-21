using MediatR;
using Microsoft.AspNetCore.Identity;
using DatingApp.Entities;

public class EditRolesHandler(UserManager<AppUser> userManager) : IRequestHandler<EditRolesCommand, bool>
{
    public async Task<bool> Handle(EditRolesCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Roles)) return false;

        var selectedRoles = request.Roles.Split(",").ToArray();
        var user = await userManager.FindByNameAsync(request.Username);

        if (user == null) return false;

        var userRoles = await userManager.GetRolesAsync(user);

        var addResult = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!addResult.Succeeded) return false;

        var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        return removeResult.Succeeded;
    }
}