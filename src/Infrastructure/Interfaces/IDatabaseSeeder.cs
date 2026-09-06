namespace Infrastructure.Interfaces;

public interface IDatabaseSeeder
{
    Task SeedDatabase();
    Task SeedRoles(IServiceProvider services);
}