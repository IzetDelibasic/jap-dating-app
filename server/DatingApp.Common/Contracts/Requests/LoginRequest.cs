namespace DatingApp.Application.Contracts.Requests;

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}