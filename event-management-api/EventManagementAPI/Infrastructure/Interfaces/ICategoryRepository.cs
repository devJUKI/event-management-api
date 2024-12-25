namespace EventManagementAPI.Infrastructure.Interfaces;

public interface ICategoryRepository
{
    Task<List<string>> GetCategories(List<int> ids, CancellationToken cancellation);
}
