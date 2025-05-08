using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;

    // 1. Add isApproved to Photo entity.
    public bool IsApproved { get; set; } = false;
}
