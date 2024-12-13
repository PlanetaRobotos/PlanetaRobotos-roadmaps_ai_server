using System.Linq.Dynamic.Core;

namespace Fleet.Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> SearchBy<T>(this IQueryable<T> query, string search, params string[] fieldNames)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        // Use EF Core "Like" method to search in any field includes

        var conditions = fieldNames.Select(field => $"{field}.Contains(@0)");

        return query.Where(string.Join(" || ", conditions), search);
    }
}