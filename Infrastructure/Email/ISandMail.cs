using Domain.Entities;
using Domain.Response;

namespace Infrastructure.Email;

public interface ISandMail
{
    Task SendAsync(User user, string password);
    Task RentalAsync(User user, Car car, Rental rental);
    Task SendPasswordChangedEmailAsync(User user, string password);
    Task SendPasswordResetEmailAsync(User user, string password);
}