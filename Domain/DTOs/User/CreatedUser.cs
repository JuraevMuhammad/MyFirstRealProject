using System.ComponentModel.DataAnnotations;
using Domain.DTOs.Car;
using Domain.DTOs.Rental;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.User;

public class CreatedUser
{
    [Required]
    public required string FullName { get; set; }
    [Required]
    public required string Email { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; }
    public IFormFile? ProfileImage { get; set; }
    public int? CarId { get; set; }
}