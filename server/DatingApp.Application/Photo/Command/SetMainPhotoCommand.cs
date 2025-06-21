using MediatR;

public class SetMainPhotoCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public int PhotoId { get; set; }
}