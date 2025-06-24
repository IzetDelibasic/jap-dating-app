using MediatR;
using Microsoft.AspNetCore.Http;
using DatingApp.Application.Contracts.Responses;

public class AddPhotoCommand : IRequest<PhotoResponse?>
{
    public string Username { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
    public List<string> Tags { get; set; } = new();
}