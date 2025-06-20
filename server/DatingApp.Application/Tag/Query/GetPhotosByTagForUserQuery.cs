using MediatR;
using DatingApp.Application.Contracts.Responses;

public class GetPhotosByTagForUserQuery : IRequest<IEnumerable<PhotoResponse>>
{
    public string Username { get; set; } = string.Empty;
    public string TagName { get; set; } = string.Empty;
}