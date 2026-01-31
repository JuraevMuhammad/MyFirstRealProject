using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ISandMail
{
    Task SendAsync(User user, string password);
}