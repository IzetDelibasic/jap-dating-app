using MediatR;

public class DeleteTagCommand : IRequest<bool>
{
    public string TagName { get; set; } = string.Empty;
}