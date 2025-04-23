using DatingApp.Entities;

namespace DatingApp.Intefaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
