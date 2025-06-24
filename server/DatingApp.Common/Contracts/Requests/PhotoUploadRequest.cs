using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Application.Contracts.Requests;

public class PhotoUploadRequest
{
    [Required]
    public IFormFile File { get; set; } = null!;

    [Required]
    [FromForm]
    public List<string> Tags { get; set; } = new List<string>();
}