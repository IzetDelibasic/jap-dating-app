using MediatR;

public class RejectPhotoCommand : IRequest<bool>
{
    public int PhotoId { get; set; }
}