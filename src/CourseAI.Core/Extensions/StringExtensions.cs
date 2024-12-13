namespace CourseAI.Core.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
        {
            return str;
        }

        Span<char> span = stackalloc char[str.Length];
        str.AsSpan().CopyTo(span);

        for (int i = 0; i < span.Length; i++)
        {
            if (i == 0 || (i > 0 && char.IsUpper(span[i])))
            {
                span[i] = char.ToLowerInvariant(span[i]);
            }
            else
            {
                break;
            }
        }

        return new string(span);
    }

    public static string LimitLength(this string str, int maxLength)
    {
        return str.Length <= maxLength ? str : str[..maxLength];
    }
}