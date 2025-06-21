using MediatR;

public class DeletePhotoCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public int PhotoId { get; set; }
}