namespace DatingApp.Entities.DTO;

public class PhotoDto
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public bool IsMain { get; set; }

    // 3. Update the PhotoDto
    public bool IsApproved { get; set; }
}
