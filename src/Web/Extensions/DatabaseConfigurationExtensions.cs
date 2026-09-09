using Infrastructure.Database.DependencyInjection;
using Ardalis.Specification;
<<<<<<< Updated upstream
=======
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Infrastructure.Database.Repositories;
>>>>>>> Stashed changes

namespace Web.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);

        services.AddScoped(typeof(IRepositoryBase<>), typeof(BaseRepository<>));

        return services;
    }
}