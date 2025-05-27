using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Common.DTO;

public class PhotoUploadDto
{
    [Required]
    public IFormFile File { get; set; } = null!;

    [Required]
    [FromForm]
    public List<string> Tags { get; set; } = new List<string>();
}
