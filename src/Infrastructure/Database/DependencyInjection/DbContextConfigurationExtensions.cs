using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database.DependencyInjection;

public static class DbContextConfigurationExtensions
{
    private const string CONNECTION_STRING_NAME = "DefaultConnection";

    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString(CONNECTION_STRING_NAME)
            )
        );

        return services;
    }
}