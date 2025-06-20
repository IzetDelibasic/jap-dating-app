namespace DatingApp.Application.Contracts.Responses;

public class PhotoApprovalStatsResponse
{
    public string? Username { get; set; }
    public int ApprovedPhotos { get; set; }
    public int UnapprovedPhotos { get; set; }
}