using EventManagementAPI.Core.Interfaces.Authentication;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Models.Authentication;
using EventManagementAPI.Domain.Services;
using EventManagementAPI.Infrastructure.Authentication;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Repositories;
using EventManagementAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventManagementAPI;

public static class DependencyInjection
{
    public static void AddEventManagementApi(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString")
            ?? throw new Exception("Connection string is null");
        
        services.AddDbContext<EventManagementDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddAuth(configuration);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAuthenticationInfrastructureService, AuthenticationInfrastructureService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
    }

    private static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SECTION_KEY, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
    }
}
