namespace DatingApp.Entities.DTO;

public class PhotoForApprovalDto
{
    // 8. Add a PhotoForApprovalDto with the Photo Id, the Url, the Username and the isApproved status
    public int Id { get; set; }
    public required string Url { get; set; }
    public string? Username { get; set; }
    public bool IsApproved { get; set; }
}
