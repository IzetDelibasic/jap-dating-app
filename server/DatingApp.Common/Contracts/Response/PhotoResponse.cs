namespace DatingApp.Application.Contracts.Responses;

public class PhotoResponse
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public bool IsMain { get; set; }
    public bool IsApproved { get; set; }
}