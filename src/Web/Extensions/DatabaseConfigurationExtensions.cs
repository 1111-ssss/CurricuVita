using Infrastructure.Database.DependencyInjection;
using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;

namespace Web.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

        return services;
    }
}