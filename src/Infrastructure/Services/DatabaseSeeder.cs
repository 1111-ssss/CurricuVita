using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly UserManager<User> _userManager;

    public DatabaseSeeder(
        AppDbContext context,
        RoleManager<IdentityRole<int>> roleManager,
        UserManager<User> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedDatabase()
    {
        await SeedAttributes();
        await SeedAdminUser();
    }

    private async Task SeedAttributes()
    {
        var attributes = new List<AttributeDefinition>
        {
            new() { Category = "Me", Name = "First Name", DataType = AttributeDataType.String, Description = "Candidate's first name" },
            new() { Category = "Me", Name = "Last Name", DataType = AttributeDataType.String, Description = "Candidate's last name" },
            new() { Category = "Me", Name = "Location", DataType = AttributeDataType.String, Description = "Candidate's current location" },
            new() { Category = "Me", Name = "Photo", DataType = AttributeDataType.Image, Description = "Candidate's professional photo" }
        };

        foreach (var attribute in attributes)
        {
            if (!await _context.AttributeDefinitions.AnyAsync(a => a.Name == attribute.Name))
            {
                attribute.CreatedAt = DateTime.UtcNow;
                _context.AttributeDefinitions.Add(attribute);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedAdminUser()
    {
        const string adminEmail = "admin@curricuvita.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "System",
                Location = "Remote",
                CreatedAt = DateTime.UtcNow,
                Version = 1
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, UserRoles.Administrator);
            }
        }
    }

    public async Task SeedRoles()
    {
        string[] roles = {
            UserRoles.Candidate,
            UserRoles.Recruiter,
            UserRoles.Administrator
        };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }
}
