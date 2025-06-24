using DatingApp.Application.Contracts.Requests;
using DatingApp.Application.Contracts.Responses;
using MediatR;

public class RegisterUserCommand : IRequest<UserResponse>
{
    public RegisterUserRequest Request { get; set; } = null!;
}