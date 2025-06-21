using MediatR;
using DatingApp.Application.Contracts.Responses;

public class GetPhotosByUserIdQuery : IRequest<IEnumerable<PhotoResponse>>
{
    public int UserId { get; set; }
}