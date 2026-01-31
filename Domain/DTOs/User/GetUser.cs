using Domain.Enums;

namespace Domain.DTOs.User;

public class GetUser
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? ProfileImage { get; set; } = string.Empty;
    public int? CarId { get; set; }
}