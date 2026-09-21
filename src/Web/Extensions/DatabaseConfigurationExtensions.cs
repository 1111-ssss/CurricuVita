using Infrastructure.Database.DependencyInjection;
using Ardalis.Specification;
using Domain.Interfaces.Database;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Infrastructure.Database.Repositories;

namespace Web.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.ConfigureDbContext(connectionString!);

        services.AddScoped(typeof(IRepositoryBase<>), typeof(BaseRepository<>));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IAttributeRepository, AttributeRepository>();
        services.AddScoped<IUserAttributeValueRepository, UserAttributeValueRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();

        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        return services;
    }
}