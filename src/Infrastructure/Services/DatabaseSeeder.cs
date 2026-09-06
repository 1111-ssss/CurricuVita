using Domain.Constants;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public class DatabaseSeeder : IDatabaseSeeder
{
    public async Task SeedDatabase()
    {
        throw new NotImplementedException();
    }

    public async Task SeedRoles(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        string[] roles = {
            UserRoles.Candidate,
            UserRoles.Recruiter,
            UserRoles.Administrator
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }
}