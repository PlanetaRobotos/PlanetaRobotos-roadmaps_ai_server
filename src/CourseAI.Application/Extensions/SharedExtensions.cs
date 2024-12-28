namespace CourseAI.Application.Extensions;

public static class SharedExtensions
{
    public static string ToUsername(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email address is null or empty.");
        }

        var parts = email.Split('@');

        if (parts.Length != 2)
        {
            throw new FormatException("Invalid email format.");
        }

        return parts[0];
    }
}