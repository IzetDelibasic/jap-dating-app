using DatingApp.Entities;

namespace DatingApp.Core.Entities;

public class PhotoTag
{
    public int PhotoId { get; set; }
    public Photo? Photo { get; set; }
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
}
