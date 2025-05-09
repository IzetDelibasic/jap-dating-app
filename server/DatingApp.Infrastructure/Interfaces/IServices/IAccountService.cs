using DatingApp.Entities.DTO;

namespace DatingApp.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto?> Register(RegisterDto registerDto);
        Task<UserDto?> Login(LoginDto loginDto);
    }
}