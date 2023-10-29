namespace Services.Common;

public record Pagination
{
    private const int MaxPageSize = 20;
    private const int MinPageNumber = 1;
    
    public Pagination()
    {
        PageNumber = MinPageNumber;
        PageSize = MaxPageSize;
    }
    
    public Pagination(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < MinPageNumber ? MinPageNumber : pageNumber;
        PageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
    }
    
    public int PageNumber { get; }
    public int PageSize { get; }
}