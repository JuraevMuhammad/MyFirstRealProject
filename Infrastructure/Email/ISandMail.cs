using Domain.Entities;

namespace Infrastructure.Email;

public interface ISandMail
{
    Task SendAsync(User user, string password);
    Task RentalAsync(User user, Car car, Rental rental);
    Task SendPasswordChangedEmailAsync(User user, string password);
    Task<string> SendPasswordResetEmailAsync(User user);
}