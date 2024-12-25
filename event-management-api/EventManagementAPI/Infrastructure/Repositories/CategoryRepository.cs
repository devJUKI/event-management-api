using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly EventManagementDbContext _dbContext;

    public CategoryRepository(EventManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<string>> GetCategories(List<int> ids, CancellationToken cancellation)
    {
        return _dbContext.Categories
            .Where(c => ids.Contains(c.Id))
            .Select(c => c.Name)
            .ToListAsync(cancellation);
    }
}
