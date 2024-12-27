using EventManagementAPI.Domain.Extensions;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Helpers;

namespace EventManagementAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", async (IEventManagementService service) =>
        {
            var response = await service.GetCategories();

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .WithTags("Categories");
    }
}
