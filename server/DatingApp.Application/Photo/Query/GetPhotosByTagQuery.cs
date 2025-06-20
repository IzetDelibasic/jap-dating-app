using MediatR;
using DatingApp.Application.Contracts.Responses;

public class GetPhotosByTagQuery : IRequest<IEnumerable<PhotoResponse>>
{
    public int TagId { get; set; }
}