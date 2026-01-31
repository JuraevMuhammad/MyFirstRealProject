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
    
    public async Task SendAsync(User user, string password)
    {
        Console.WriteLine($"FROM = '{_options.From}'");
        Console.WriteLine($"TO = '{user.Email}'");
        
        MailAddress fromMailAddress = new(_options.From, _options.UserName);
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
}