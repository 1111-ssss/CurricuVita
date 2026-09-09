using Domain.Constants;
using Domain.Interfaces.Identity;
using Infrastructure.Database;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace Web.Extensions;

public static class IdentityConfigurationExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddIdentity<User, IdentityRole<int>>(options =>
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
                var section = configuration.GetSection("Authentication:Google");

                options.ClientId = section["ClientId"]!;
                options.ClientSecret = section["ClientSecret"]!;
            })
            .AddGitHub(options =>
            {
                var section = configuration.GetSection("Authentication:GitHub");

                options.ClientId = section["ClientId"]!;
                options.ClientSecret = section["ClientSecret"]!;
                options.Scope.Add("user:email");
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireCandidate", policy =>
                policy.RequireRole(UserRoles.Candidate));

            options.AddPolicy("RequireRecruiter", policy =>
                policy.RequireRole(UserRoles.Recruiter));

            options.AddPolicy("RequireAdmin", policy =>
                policy.RequireRole(UserRoles.Administrator));

            options.AddPolicy("RequireRecruiterOrAdmin", policy =>
                policy.RequireRole(UserRoles.Recruiter, UserRoles.Administrator));
        });

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}