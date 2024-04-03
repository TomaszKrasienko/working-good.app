using wg.shared.abstractions.Pagination;

namespace wg.shared.infrastructure.Pagination.Mappers;

public static class Extensions
{
    public static MetaDataDto AsMetaData<T>(this PagedList<T> pagedList)
        => new MetaDataDto()
        {
            CurrentPage = pagedList.CurrentPage,
            TotalPages = pagedList.TotalPages,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            HasPrevious = pagedList.HasPrevious,
            HasNext = pagedList.HasNext
        };
}