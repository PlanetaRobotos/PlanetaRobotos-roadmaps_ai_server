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
    
    public static string ToUrl(string title)
    {
        // Convert to lowercase and replace spaces with dashes
        var processedTitle = title
            .ToLower()
            .Trim()
            // Replace spaces and invalid chars with dash
            .Replace(" ", "-")
            .Replace("&", "and")
            // Remove any other special characters
            .Replace("'", "")
            .Replace("\"", "")
            .Replace("@", "at")
            .Replace(":", "")
            .Replace(";", "")
            .Replace("?", "")
            .Replace("!", "")
            .Replace("#", "")
            .Replace("%", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("+", "-plus")
            .Replace("=", "-equals");

        // Remove any double dashes
        while (processedTitle.Contains("--"))
        {
            processedTitle = processedTitle.Replace("--", "-");
        }

        // Get first 50 chars of the title to keep it reasonable
        if (processedTitle.Length > 50)
        {
            processedTitle = processedTitle.Substring(0, 50).TrimEnd('-');
        }

        // Add short unique identifier at the end
        var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
    
        return $"{processedTitle}_{uniqueId}.png";
    }
    
    public static string FromUrl(string fileName)
    {
        // Remove the extension
        fileName = Path.GetFileNameWithoutExtension(fileName);

        // Remove the unique identifier (last 8 characters + underscore)
        var withoutGuid = fileName.Substring(0, fileName.Length - 9);

        // Convert dashes back to spaces
        var processedTitle = withoutGuid
            .Replace("-", " ")
            // Convert back special characters
            .Replace("and", "&")
            .Replace("at", "@")
            .Replace("-plus", "+")
            .Replace("-equals", "=");

        // Proper case the title (capitalize first letter of each word)
        var titleCase = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(processedTitle);

        return titleCase;
    }
}