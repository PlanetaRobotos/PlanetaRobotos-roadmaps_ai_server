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
    
    public static bool IsDefinedIgnoreCase<TEnum>(this TEnum enumType, string value) where TEnum : Enum
    {
        return Enum.GetNames(typeof(TEnum)).Any(e => e.Equals(value, StringComparison.OrdinalIgnoreCase));
    }
    
    public static string FirstLetterToUpper(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return char.ToUpper(str[0]) + str.Substring(1);
    }
}