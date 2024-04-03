namespace wg.shared.abstractions.Pagination;

public abstract record PaginationDto
{
    public int PageNumber { get; init; } = 1;
    private const int MaxPageSize = 30;
    private readonly int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = MaxPageSize > value ? value : MaxPageSize;
    }
}