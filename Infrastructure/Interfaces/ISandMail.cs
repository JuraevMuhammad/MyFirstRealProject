using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ISandMail
{
    Task SendAsync(User user, string password);
    Task RentalAsync(User user, Car car, Rental rental);
    Task SendPasswordChangedEmailAsync(User user, string password);

}