using DatingApp.Application.Contracts.Requests;
using DatingApp.Application.Contracts.Responses;
using MediatR;

public class LoginUserQuery : IRequest<UserResponse?>
{
    public LoginRequest Request { get; set; } = null!;
}