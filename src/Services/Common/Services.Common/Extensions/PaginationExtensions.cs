namespace Services.Common.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paged<T>(this IQueryable<T> queryable,Pagination pagination)
    {
        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        return queryable
            .Skip(skip)
            .Take(pagination.PageSize);
    }
}