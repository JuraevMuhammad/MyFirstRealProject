namespace Infrastructure.Email;

public class EmailOptions
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string From { get; set; } =  string.Empty;
}