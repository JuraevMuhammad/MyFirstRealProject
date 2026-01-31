using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.User;

public class UpdateUser
{
    public string? FullName { get; set; }
    public IFormFile? ProfileImage { get; set; }
    public int? CarId { get; set; }
}