using Infrastructure.Database.DependencyInjection;
using Ardalis.Specification;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Infrastructure.Database.Repositories;

namespace Web.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);

        services.AddScoped(typeof(IRepositoryBase<>), typeof(BaseRepository<>));

        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        return services;
    }
}