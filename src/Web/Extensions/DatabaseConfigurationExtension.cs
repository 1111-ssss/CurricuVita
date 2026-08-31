using Infrastructure.Database.DependencyInjection;

namespace Web.Extensions;

public static class DatabaseConfigurationExtension
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);

        return services;
    }
}