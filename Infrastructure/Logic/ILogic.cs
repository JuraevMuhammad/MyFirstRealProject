namespace Infrastructure.Logic;

public interface ILogic
{
    bool ValidatorPassword(string password);
    bool ValidatorEmail(string email);
}