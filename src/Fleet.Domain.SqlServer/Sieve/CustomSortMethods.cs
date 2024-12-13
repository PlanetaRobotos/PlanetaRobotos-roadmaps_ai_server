using Sieve.Services;

namespace Fleet.Domain.Sieve;

/// <summary>
/// Read more: https://github.com/Biarity/Sieve?tab=readme-ov-file#add-custom-sortfilter-methods
/// </summary>
public class CustomSortMethods : ISieveCustomSortMethods
{
    // public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc) // The method is given an indicator of whether to use ThenBy(), and if the query is descending 
    // {
    //     var result = useThenBy ?
    //         ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount) : // ThenBy only works on IOrderedQueryable<TEntity>
    //         source.OrderBy(p => p.LikeCount)
    //             .ThenBy(p => p.CommentCount)
    //             .ThenBy(p => p.DateCreated);
    //
    //     return result; // Must return modified IQueryable<TEntity>
    // }
}