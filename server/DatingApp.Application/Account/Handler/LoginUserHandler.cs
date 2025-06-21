using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DatingApp.Intefaces;

public class LoginUserHandler(UserManager<AppUser> userManager, ITokenService tokenService
    ) : IRequestHandler<LoginUserQuery, UserResponse?>
{
    public async Task<UserResponse?> Handle(LoginUserQuery query, CancellationToken cancellationToken)
    {
        var req = query.Request;
        var user = await userManager.Users
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.NormalizedUserName == req.Username.ToUpper());

        if (user == null || user.UserName == null)
            return null;

        var result = await userManager.CheckPasswordAsync(user, req.Password);

        if (!result)
            return null;

        return new UserResponse
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = await tokenService.CreateToken(user),
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }
}