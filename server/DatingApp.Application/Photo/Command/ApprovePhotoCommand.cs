using MediatR;

public class ApprovePhotoCommand : IRequest<bool>
{
    public int PhotoId { get; set; }
}