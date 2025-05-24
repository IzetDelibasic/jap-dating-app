using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Common.DTO;

public class PhotoUploadDto
{
    [Required]
    public IFormFile File { get; set; } = null!;

    [Required]
    public List<string> Tags { get; set; } = new List<string>();
}
