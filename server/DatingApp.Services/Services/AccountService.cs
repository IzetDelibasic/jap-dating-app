using AutoMapper;
using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Intefaces;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services
{
    public class AccountService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService) : IAccountService
    {
        public async Task<UserDto?> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return null;

            if (!DateOnly.TryParse(registerDto.DateOfBirth, out var parsedDateOfBirth))
                throw new ArgumentException("Invalid DateOfBirth format");

            var user = mapper.Map<AppUser>(registerDto);
            user.DateOfBirth = parsedDateOfBirth;
            user.UserName = registerDto.Username.ToLower();

            var result = await userManager.CreateAsync(user, registerDto.Password!);

            if (!result.Succeeded) return null;

            return new UserDto
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                Gender = user.Gender,
                KnownAs = user.KnownAs
            };
        }

        public async Task<UserDto?> Login(LoginDto loginDto)
        {
            var user = await userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.Username.ToUpper());

            if (user == null || user.UserName == null) return null;

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) return null;

            return new UserDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Token = await tokenService.CreateToken(user),
                Gender = user.Gender,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
        }
    }
}