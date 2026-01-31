namespace Infrastructure.Logic;

public class Logic : ILogic
{
    #region MyPassword

    public bool ValidatorPassword(string password)
    {
        int p = 0, cnt = 0, sum = 0;
        foreach (var s in password)
        {
            p++;
            if (s >= 65 && s <= 90 || s>=97 && s<=122)
            {
                cnt++;
            }
            if (s >= 33 && s <=64)
            {
                sum++;        
            }
        }
        return p >= 8 && cnt >= 1 && sum >=1;
    }

    #endregion

    public bool ValidatorEmail(string email)
    {
        throw new NotImplementedException();
    }
}