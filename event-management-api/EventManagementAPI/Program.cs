using EventManagementAPI.Middleware;
using EventManagementAPI.Endpoints;

namespace EventManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddEventManagementApi(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAuthenticationEndpoints();
            app.MapUserManagementEndpoints();
            app.MapEventManagementEndpoints();
            app.MapCategoryEndpoints();

            app.Run();
        }
    }
}
