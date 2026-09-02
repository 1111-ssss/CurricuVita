using Application;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Web.Extensions;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Infrastructure services
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
        
        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly)
        );

        return services;
    }
}