namespace DatingApp.Application.Contracts.Requests;

public class GenerateMessageRequest
{
    public string Interests { get; set; } = string.Empty;
    public string LookingFor { get; set; } = string.Empty;
}