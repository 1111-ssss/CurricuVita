using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace Web.Extensions;

public static class IdentityConfigurationExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            var section = configuration.GetSection("Identity:Cookie");

            options.LoginPath = section.GetValue<string>("LoginPath");
            options.LogoutPath = section.GetValue<string>("LogoutPath");
            options.AccessDeniedPath = section.GetValue<string>("AccessDeniedPath");
            options.ExpireTimeSpan = section.GetValue<TimeSpan>("ExpireTimeSpan");
            options.SlidingExpiration = section.GetValue<bool>("SlidingExpiration");
        });

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            })
            .AddGitHub(options =>
            {
                options.ClientId = configuration["Authentication:GitHub:ClientId"]!;
                options.ClientSecret = configuration["Authentication:GitHub:ClientSecret"]!;
                options.Scope.Add("user:email");
            });

        return services;
    }
}