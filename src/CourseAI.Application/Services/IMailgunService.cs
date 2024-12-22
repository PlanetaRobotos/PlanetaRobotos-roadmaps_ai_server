namespace CourseAI.Application.Services;

public interface IMailgunService
{
    Task SendEmailAsync(string to, string subject, string htmlContent);
}
