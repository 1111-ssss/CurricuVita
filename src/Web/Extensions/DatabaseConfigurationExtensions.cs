using Infrastructure.Database.DependencyInjection;
using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Web.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        return services;
    }
}