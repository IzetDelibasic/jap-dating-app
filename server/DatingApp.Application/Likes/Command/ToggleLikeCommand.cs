using MediatR;

public class ToggleLikeCommand : IRequest<bool>
{
    public int SourceUserId { get; set; }
    public int TargetUserId { get; set; }
}