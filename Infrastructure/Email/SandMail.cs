using System.Net;
using System.Net.Mail;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Email;

public class SandMail : ISandMail
{
    #region Constructors

    private readonly EmailOptions _options;

    public SandMail(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }

    #endregion

    #region Send

    public async Task SendAsync(User user, string password)
    {
        MailAddress fromMailAddress = new(_options.From, "Car Rental Service");
        MailAddress toAddress = new(user.Email, user.FullName);

        using var mailMessage = new MailMessage(fromMailAddress, toAddress)
        {
            Subject = user.Email,
            Body = $"Hello {user.FullName}!\nYour login: {user.Email}\nYour password: {password}"
        };

        using var smtp = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _options.UserName,
                _options.Password),
        };
        
        await smtp.SendMailAsync(mailMessage);
    }

    #endregion

    #region SandEmailRental

    public async Task RentalAsync(User user, Car car, Rental rental)
    {
        MailAddress from = new (_options.From, "Car Rental Service");
        MailAddress to = new(user.Email, user.FullName);

        using var mailMessage = new MailMessage(from, to)
        {
            Subject = user.Email,
            Body = $"""
                        -Здраствуйте {user.FullName}
                    Вы взяли аренду машину {rental.CarId}
                    Дата Аренды: {rental.StartDate.Date}
                    Цена за день: {car.DailyPrice}
                    """
        };

        using var smtp = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _options.UserName,
                _options.Password)

        };
        
        await smtp.SendMailAsync(mailMessage);
    }

    #endregion
}