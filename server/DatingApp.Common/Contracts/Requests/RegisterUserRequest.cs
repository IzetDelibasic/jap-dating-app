using System.ComponentModel.DataAnnotations;

namespace DatingApp.Application.Contracts.Requests;

public class RegisterUserRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string? KnownAs { get; set; }
    [Required]
    public string? Gender { get; set; }
    [Required]
    public string? DateOfBirth { get; set; }
    [Required]
    public string? City { get; set; }
    [Required]
    public string? Country { get; set; }
    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string? Password { get; set; }
}