using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mime;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    [Required]
    public required string FullName { get; set; }
    [Required]
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public string? ProfileImage { get; set; }
    public int? CarId { get; set; }
    
    public Car? Car { get; set; }
    public Rental? Rental { get; set; }
}