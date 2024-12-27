using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface ICategoryRepository
{
    Task<List<CategoryDomainModel>> GetCategories(List<int>? ids = null, CancellationToken cancellation = default);
}
