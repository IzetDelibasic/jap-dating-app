namespace DatingApp.Application.Contracts.Responses;

public class PhotoForApprovalResponse
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public string? Username { get; set; }
    public bool IsApproved { get; set; }
}