using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database.DependencyInjection;

public static class DbContextConfigurationExtensions
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        return services;
    }
}
