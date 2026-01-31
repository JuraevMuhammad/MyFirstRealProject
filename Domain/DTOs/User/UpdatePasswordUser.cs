namespace Domain.DTOs.User;

public class UpdatePasswordUser
{
    public string? LastPassword { get; set; }
    public string? NewPassword { get; set; }
}