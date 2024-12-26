namespace EventManagementAPI.ViewModels.Common;

public record PaginatedListResponseViewModel<T>(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalCount,
    bool HasPreviousPage,
    bool HasNextPage,
    List<T> Items);
