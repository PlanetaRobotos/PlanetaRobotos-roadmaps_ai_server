namespace CourseAI.Application.Options;

public class EmailOptions
{
    public string SenderEmail { get; set; } = null!;
    public string Sender { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}