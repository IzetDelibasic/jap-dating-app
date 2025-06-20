using AutoMapper;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;
using DatingApp.Exceptions;
using DatingApp.Intefaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class RegisterUserHandler(UserManager<AppUser> userManager, IMapper mapper,
    ITokenService tokenService) : IRequestHandler<RegisterUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var dto = command.Request;
        if (await userManager.Users.AnyAsync(x => x.NormalizedUserName == dto.Username.ToUpper()))
            throw new BadRequestException("User already exists");

        if (!DateOnly.TryParse(dto.DateOfBirth, out var parsedDateOfBirth))
            throw new ValidationException("Invalid DateOfBirth format");

        var user = mapper.Map<AppUser>(dto);
        user.DateOfBirth = parsedDateOfBirth;
        user.UserName = dto.Username.ToLower();

        var result = await userManager.CreateAsync(user, dto.Password!);

        if (!result.Succeeded)
            throw new BadRequestException("Failed to register user");

        return new UserResponse
        {
            Username = user.UserName!,
            Token = await tokenService.CreateToken(user),
            Gender = user.Gender!,
            KnownAs = user.KnownAs!
        };
    }
}