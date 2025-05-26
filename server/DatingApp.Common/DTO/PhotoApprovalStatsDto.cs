namespace DatingApp.Common.DTO;

public class PhotoApprovalStatsDto
{
    public string? Username { get; set; }
    public int ApprovedPhotos { get; set; }
    public int UnapprovedPhotos { get; set; }
}
