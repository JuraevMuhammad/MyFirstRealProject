using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.User;

public class LoginUser
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}