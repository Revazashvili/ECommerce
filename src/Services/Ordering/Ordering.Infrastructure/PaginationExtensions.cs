namespace Ordering.Infrastructure;

internal static class PaginationExtensions
{
    internal static IQueryable<T> Paged<T>(this IQueryable<T> queryable, int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        
        return queryable.Skip(skip).Take(pageSize);
    }
}