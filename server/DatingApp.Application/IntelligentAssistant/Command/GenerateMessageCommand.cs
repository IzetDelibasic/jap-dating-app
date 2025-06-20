using MediatR;

public class GenerateMessageCommand : IRequest<string>
{
    public string Interests { get; set; } = string.Empty;
    public string LookingFor { get; set; } = string.Empty;
}