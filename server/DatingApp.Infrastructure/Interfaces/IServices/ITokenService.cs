using DatingApp.Entities;

namespace DatingApp.Intefaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}
