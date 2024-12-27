using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly EventManagementDbContext _dbContext;

    public CategoryRepository(EventManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<CategoryDomainModel>> GetCategories(List<int>? ids = null, CancellationToken cancellation = default)
    {
        var query = _dbContext.Categories.AsQueryable();

        if (ids != null && ids.Count != 0)
        {
            query = query.Where(c => ids.Contains(c.Id));
        }

        return query
            .Select(c => new CategoryDomainModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync(cancellation);
    }
}
