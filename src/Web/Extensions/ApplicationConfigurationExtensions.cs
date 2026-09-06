using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Web.Extensions;

public static class ApplicationConfigurationExtensions
{
    public static async Task<WebApplication> AddApplicationConfiguration(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();

            if (app.Environment.IsDevelopment())
            {
                var dbSeeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
                await dbSeeder.SeedRoles(scope.ServiceProvider);
                await dbSeeder.SeedDatabase();
            }
        }        

        return app;
    }
}