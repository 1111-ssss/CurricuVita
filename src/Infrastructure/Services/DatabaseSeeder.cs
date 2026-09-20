using System.Text.Json;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        AppDbContext context,
        RoleManager<IdentityRole<int>> roleManager,
        UserManager<User> userManager,
        ILogger<DatabaseSeeder> logger
    )
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task SeedDatabase()
    {
        await SeedAttributes();
        await SeedTags();
        await SeedBadges();
        await SeedDemoUsers();
        await SeedPositions();
        await SeedProjectsAndAttributeValues();
        await SeedCvsLikesAndDiscussions();
        _logger.LogInformation("Database seeding completed.");
    }

    private async Task SeedAttributes()
    {
        var attributes = new List<AttributeDefinition>
        {
            new() { Category = "Me", Name = "First Name", DataType = AttributeDataType.String, Description = "Candidate's first name" },
            new() { Category = "Me", Name = "Last Name", DataType = AttributeDataType.String, Description = "Candidate's last name" },
            new() { Category = "Me", Name = "Location", DataType = AttributeDataType.String, Description = "Candidate's current location" },
            new() { Category = "Me", Name = "Photo", DataType = AttributeDataType.Image, Description = "Candidate's professional photo" },

            new() { Category = "Personal Information", Name = "Birth Date", DataType = AttributeDataType.Date, Description = "Date of birth" },
            new() { Category = "Personal Information", Name = "About Me", DataType = AttributeDataType.Text, Description = "Short bio in Markdown" },
            new() { Category = "Personal Information", Name = "Remote Work Availability", DataType = AttributeDataType.Boolean, Description = "Ready to work remotely" },
            new() { Category = "Personal Information", Name = "Availability Period", DataType = AttributeDataType.Period, Description = "When candidate is available" },

            new() { Category = "Skills", Name = "English Level", DataType = AttributeDataType.Dropdown, Description = "English proficiency", OptionsJson = JsonSerializer.Serialize(new[] { "A1", "A2", "B1", "B2", "C1", "C2" }) },
            new() { Category = "Skills", Name = "IELTS Score", DataType = AttributeDataType.Numeric, Description = "IELTS band score 0-9", MinValue = 0, MaxValue = 9 },
            new() { Category = "Skills", Name = "Programming Experience", DataType = AttributeDataType.Numeric, Description = "Years of programming experience", MinValue = 0, MaxValue = 50 },
            new() { Category = "Skills", Name = "Presentation Skills", DataType = AttributeDataType.Dropdown, Description = "Presentation skill level", OptionsJson = JsonSerializer.Serialize(new[] { "Beginner", "Intermediate", "Advanced", "Expert" }) },

            new() { Category = "Certification", Name = "CAP", DataType = AttributeDataType.Dropdown, Description = "Certificates level: None / Essentials / Pro / Expert", OptionsJson = JsonSerializer.Serialize(new[] { "None", "Essentials", "Pro", "Expert" }) },

            new() { Category = "Domain Knowledge", Name = "Python", DataType = AttributeDataType.Dropdown, Description = "Python skill level", OptionsJson = JsonSerializer.Serialize(new[] { "None", "Basic", "Intermediate", "Advanced", "Expert" }) },
            new() { Category = "Domain Knowledge", Name = "GPA", DataType = AttributeDataType.Numeric, Description = "Grade point average 0-5", MinValue = 0, MaxValue = 5 },
            new() { Category = "Domain Knowledge", Name = "Hadoop", DataType = AttributeDataType.Dropdown, Description = "Hadoop knowledge level", OptionsJson = JsonSerializer.Serialize(new[] { "None", "Basic", "Intermediate", "Advanced" }) },

            new() { Category = "Personal Information", Name = "Person Age", DataType = AttributeDataType.Numeric, Description = "Age of the person", MinValue = 0, MaxValue = 150 },
            new() { Category = "Personal Information", Name = "Person Name", DataType = AttributeDataType.String, Description = "Display name" },
            new() { Category = "Personal Information", Name = "Address", DataType = AttributeDataType.String, Description = "Postal address" },

            new() { Category = "Soft Skills", Name = "Communication", DataType = AttributeDataType.Dropdown, Description = "Communication skills", OptionsJson = JsonSerializer.Serialize(new[] { "Beginner", "Intermediate", "Advanced", "Expert" }) },
            new() { Category = "Soft Skills", Name = "Dancing Skills", DataType = AttributeDataType.Text, Description = "Free-form dancing skills (attributes are independent by design)" },
        };

        foreach (var attribute in attributes)
        {
            if (!await _context.AttributeDefinitions.AnyAsync(a => a.Name == attribute.Name))
            {
                attribute.CreatedAt = DateTime.UtcNow;
                attribute.Version = 1;
                _context.AttributeDefinitions.Add(attribute);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedTags()
    {
        string[] tags =
        [
            "Python", "Data Engineering", "Hadoop", "Machine Learning", "SQL",
            "C#", "ASP.NET Core", "EF Core", "PostgreSQL", "Docker",
            "React", "TypeScript", "JavaScript"
        ];

        foreach (var name in tags)
        {
            if (!await _context.Tags.AnyAsync(t => t.Name == name))
            {
                _context.Tags.Add(new Tag { Name = name });
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedBadges()
    {
        var badges = new List<Badge>
        {
            new() { Code = "first-cv", Title = "First CV", Description = "Created first CV", SvgTemplate = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100'><circle cx='50' cy='50' r='45' fill='#4caf50'/><text x='50' y='62' font-size='40' text-anchor='middle' fill='white'>1</text></svg>" },
            new() { Code = "five-cvs", Title = "5 CVs", Description = "Created 5 CVs", SvgTemplate = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100'><circle cx='50' cy='50' r='45' fill='#2196f3'/><text x='50' y='62' font-size='40' text-anchor='middle' fill='white'>5</text></svg>" },
            new() { Code = "ten-projects", Title = "10 Projects", Description = "Added 10 projects", SvgTemplate = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100'><rect x='5' y='5' width='90' height='90' rx='15' fill='#ff9800'/><text x='50' y='62' font-size='35' text-anchor='middle' fill='white'>10</text></svg>" },
            new() { Code = "likes-25", Title = "25 Likes", Description = "Collected 25 likes on CVs", SvgTemplate = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100'><path d='M50 88 C20 60 10 40 20 28 C30 16 45 20 50 32 C55 20 70 16 80 28 C90 40 80 60 50 88Z' fill='#e91e63'/></svg>" },
            new() { Code = "early-adopter", Title = "Early Adopter", Description = "Joined during beta", SvgTemplate = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100'><polygon points='50,5 61,35 93,35 67,55 76,86 50,67 24,86 33,55 7,35 39,35' fill='#9c27b0'/></svg>" },
        };

        foreach (var badge in badges)
        {
            if (!await _context.Badges.AnyAsync(b => b.Code == badge.Code))
            {
                _context.Badges.Add(badge);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedDemoUsers()
    {
        await SeedAdminUser();

        await EnsureUser(
            email: "recruiter@curricuvita.com",
            role: UserRoles.Recruiter,
            firstName: "Anna",
            lastName: "Recruiter",
            location: "Warsaw");

        await EnsureUser(
            email: "candidate1@curricuvita.com",
            role: UserRoles.Candidate,
            firstName: "Ivan",
            lastName: "Petrov",
            location: "Tashkent");

        await EnsureUser(
            email: "candidate2@curricuvita.com",
            role: UserRoles.Candidate,
            firstName: "Mary",
            lastName: "Smith",
            location: "Berlin");

        await EnsureUser(
            email: "candidate3@curricuvita.com",
            role: UserRoles.Candidate,
            firstName: "Ellen",
            lastName: "Doe",
            location: "Frisco");

        var candidate1 = await _userManager.FindByEmailAsync("candidate1@curricuvita.com");
        var firstCvBadge = await _context.Badges.FirstOrDefaultAsync(b => b.Code == "early-adopter");
        if (candidate1 != null && firstCvBadge != null
            && !await _context.UserBadges.AnyAsync(ub => ub.UserId == candidate1.Id && ub.BadgeId == firstCvBadge.Id))
        {
            _context.UserBadges.Add(new UserBadge
            {
                UserId = candidate1.Id,
                BadgeId = firstCvBadge.Id,
                EarnedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }

    private async Task<User> EnsureUser(string email, string role, string firstName, string lastName, string location)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            if (!await _userManager.IsInRoleAsync(user, role))
            {
                var existingRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (!existingRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to add existing user '{email}' to role '{role}': {DescribeErrors(existingRoleResult)}");
                }
            }
            return user;
        }

        user = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FirstName = firstName,
            LastName = lastName,
            Location = location,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Version = 1,
        };

        var result = await _userManager.CreateAsync(user, "Demo123!");
        if (!result.Succeeded)
        {
            _context.Entry(user).State = EntityState.Detached;
            throw new InvalidOperationException(
                $"Failed to create demo user '{email}': {DescribeErrors(result)}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to add demo user '{email}' to role '{role}': {DescribeErrors(roleResult)}");
        }

        return user;
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
                UpdatedAt = DateTime.UtcNow,
                Version = 1,
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin123!");
            if (!result.Succeeded)
            {
                _context.Entry(adminUser).State = EntityState.Detached;
                throw new InvalidOperationException(
                    $"Failed to create admin user '{adminEmail}': {DescribeErrors(result)}");
            }

            var roleResult = await _userManager.AddToRoleAsync(adminUser, UserRoles.Administrator);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to add admin user '{adminEmail}' to role '{UserRoles.Administrator}': {DescribeErrors(roleResult)}");
            }
        }
    }

    private async Task SeedPositions()
    {
        if (await _context.Positions.AnyAsync())
        {
            return;
        }

        var recruiter = await _userManager.FindByEmailAsync("recruiter@curricuvita.com")
            ?? await _userManager.FindByEmailAsync("admin@curricuvita.com");
        if (recruiter == null)
        {
            return;
        }

        var attrId = await _context.AttributeDefinitions
            .ToDictionaryAsync(a => a.Name, a => a.Id);
        var tagId = await _context.Tags
            .ToDictionaryAsync(t => t.Name, t => t.Id);

        int GetAttr(string name) => attrId[name];
        int GetTag(string name) => tagId[name];

        var now = DateTime.UtcNow;

        var positions = new List<Position>
        {
            new()
            {
                Title = "Junior Data Engineer",
                DescriptionMarkdown = "## Junior Data Engineer\nLooking for a junior engineer with **Python**, **Hadoop** and good English. Projects filtered by `Python` + `Data Engineering` (max 3).",
                Company = "Acme Data",
                Level = "Junior",
                IsPublic = true,
                MaxProjectCount = 3,
                CreatedAt = now,
                UpdatedAt = now,
                Version = 1,
                CreatedById = recruiter.Id,
                RequiredAttributes =
                {
                    new PositionAttribute { AttributeDefinitionId = GetAttr("English Level"), IsRequired = true, Order = 1 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("GPA"), IsRequired = true, Order = 2 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Python"), IsRequired = true, Order = 3 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Hadoop"), IsRequired = false, Order = 4 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("CAP"), IsRequired = false, Order = 5 },
                },
                RequiredTags =
                {
                    new PositionTag { TagId = GetTag("Python") },
                    new PositionTag { TagId = GetTag("Data Engineering") },
                },
            },
            new()
            {
                Title = "Senior Backend .NET Developer",
                DescriptionMarkdown = "## Senior Backend\nStack: **C#**, **ASP.NET Core**, **EF Core**, **PostgreSQL**, **Docker**. Public position.",
                Company = "Contoso",
                Level = "Senior",
                IsPublic = true,
                MaxProjectCount = 5,
                CreatedAt = now,
                UpdatedAt = now,
                Version = 1,
                CreatedById = recruiter.Id,
                RequiredAttributes =
                {
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Programming Experience"), IsRequired = true, Order = 1 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("English Level"), IsRequired = true, Order = 2 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("About Me"), IsRequired = false, Order = 3 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Communication"), IsRequired = false, Order = 4 },
                },
                RequiredTags =
                {
                    new PositionTag { TagId = GetTag("C#") },
                    new PositionTag { TagId = GetTag("ASP.NET Core") },
                    new PositionTag { TagId = GetTag("PostgreSQL") },
                },
            },
            new()
            {
                Title = "Frontend React Developer",
                DescriptionMarkdown = "## Frontend React\nRestricted position: only for candidates with `Remote Work Availability = true`. Projects filtered by React / TypeScript.",
                Company = "Fabrikam",
                Level = "Middle",
                IsPublic = false,
                MaxProjectCount = 3,
                CreatedAt = now,
                UpdatedAt = now,
                Version = 1,
                CreatedById = recruiter.Id,
                RequiredAttributes =
                {
                    new PositionAttribute { AttributeDefinitionId = GetAttr("English Level"), IsRequired = true, Order = 1 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Presentation Skills"), IsRequired = false, Order = 2 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Remote Work Availability"), IsRequired = true, Order = 3 },
                },
                RequiredTags =
                {
                    new PositionTag { TagId = GetTag("React") },
                    new PositionTag { TagId = GetTag("TypeScript") },
                },
                AccessRules =
                {
                    new PositionAccessRule { AttributeDefinitionId = GetAttr("Remote Work Availability"), Operator = Operator.Equal, Value = "true" },
                },
            },
            new()
            {
                Title = "Data Scientist",
                DescriptionMarkdown = "## Data Scientist\nRestricted: `IELTS > 7.0`. Projects filtered by Machine Learning / Python.",
                Company = "Acme Data",
                Level = "Senior",
                IsPublic = false,
                MaxProjectCount = 3,
                CreatedAt = now,
                UpdatedAt = now,
                Version = 1,
                CreatedById = recruiter.Id,
                RequiredAttributes =
                {
                    new PositionAttribute { AttributeDefinitionId = GetAttr("IELTS Score"), IsRequired = true, Order = 1 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Python"), IsRequired = true, Order = 2 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("GPA"), IsRequired = false, Order = 3 },
                },
                RequiredTags =
                {
                    new PositionTag { TagId = GetTag("Machine Learning") },
                    new PositionTag { TagId = GetTag("Python") },
                },
                AccessRules =
                {
                    new PositionAccessRule { AttributeDefinitionId = GetAttr("IELTS Score"), Operator = Operator.GreaterThan, Value = "7.0" },
                },
            },
            new()
            {
                Title = "Generic Employee",
                DescriptionMarkdown = "## Generic Employee\nDemo for attribute rename scenario (`Person Age` / `Person Name` / `Address`).",
                IsPublic = true,
                MaxProjectCount = null,
                CreatedAt = now,
                UpdatedAt = now,
                Version = 1,
                CreatedById = recruiter.Id,
                RequiredAttributes =
                {
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Person Age"), IsRequired = true, Order = 1 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Person Name"), IsRequired = true, Order = 2 },
                    new PositionAttribute { AttributeDefinitionId = GetAttr("Address"), IsRequired = false, Order = 3 },
                },
            },
        };

        _context.Positions.AddRange(positions);
        await _context.SaveChangesAsync();
    }

    private async Task SeedProjectsAndAttributeValues()
    {
        var candidate1 = await _userManager.FindByEmailAsync("candidate1@curricuvita.com");
        var candidate2 = await _userManager.FindByEmailAsync("candidate2@curricuvita.com");
        var candidate3 = await _userManager.FindByEmailAsync("candidate3@curricuvita.com");
        if (candidate1 == null || candidate2 == null || candidate3 == null)
        {
            return;
        }

        var attrId = await _context.AttributeDefinitions
            .ToDictionaryAsync(a => a.Name, a => a.Id);
        var tagId = await _context.Tags
            .ToDictionaryAsync(t => t.Name, t => t.Id);

        var values = new List<UserAttributeValue>
        {
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["English Level"], StringValue = "B2", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["IELTS Score"], NumericValue = 7.5m, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["Python"], StringValue = "Advanced", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["Hadoop"], StringValue = "Intermediate", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["GPA"], NumericValue = 4.5m, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["CAP"], StringValue = "Pro", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["About Me"], TextValue = "Data engineer with 3 years of **Python** and ETL experience.", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["Remote Work Availability"], BooleanValue = true, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["Birth Date"], DateValue = new DateTime(1998, 5, 12, 0, 0, 0, DateTimeKind.Utc), Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate1.Id, AttributeDefinitionId = attrId["Availability Period"], PeriodStartValue = DateTime.UtcNow.Date, PeriodEndValue = DateTime.UtcNow.Date.AddMonths(3), Version = 1, UpdatedAt = DateTime.UtcNow },

            new() { UserId = candidate2.Id, AttributeDefinitionId = attrId["Person Age"], NumericValue = 21, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate2.Id, AttributeDefinitionId = attrId["Person Name"], StringValue = "Mary", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate2.Id, AttributeDefinitionId = attrId["Programming Experience"], NumericValue = 5, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate2.Id, AttributeDefinitionId = attrId["English Level"], StringValue = "C1", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate2.Id, AttributeDefinitionId = attrId["About Me"], TextValue = "Backend developer: **C#**, ASP.NET Core, PostgreSQL.", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate2.Id, AttributeDefinitionId = attrId["Remote Work Availability"], BooleanValue = true, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate2.Id, AttributeDefinitionId = attrId["Communication"], StringValue = "Advanced", Version = 1, UpdatedAt = DateTime.UtcNow },

            new() { UserId = candidate3.Id, AttributeDefinitionId = attrId["Person Age"], NumericValue = 10, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate3.Id, AttributeDefinitionId = attrId["Person Name"], StringValue = "Ellen", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate3.Id, AttributeDefinitionId = attrId["Address"], StringValue = "Frisco", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate3.Id, AttributeDefinitionId = attrId["English Level"], StringValue = "B1", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate3.Id, AttributeDefinitionId = attrId["Presentation Skills"], StringValue = "Intermediate", Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate3.Id, AttributeDefinitionId = attrId["Remote Work Availability"], BooleanValue = true, Version = 1, UpdatedAt = DateTime.UtcNow },
            new() { UserId = candidate3.Id, AttributeDefinitionId = attrId["About Me"], TextValue = "Frontend developer: **React**, TypeScript.", Version = 1, UpdatedAt = DateTime.UtcNow },
        };

        foreach (var v in values)
        {
            if (!await _context.UserAttributeValues.AnyAsync(x => x.UserId == v.UserId && x.AttributeDefinitionId == v.AttributeDefinitionId))
            {
                _context.UserAttributeValues.Add(v);
            }
        }
        await _context.SaveChangesAsync();

        if (!await _context.Projects.AnyAsync())
        {
            var projects = new List<Project>
            {
                new()
                {
                    UserId = candidate1.Id, Title = "ETL Pipeline for Retail",
                    Location = "Tashkent",
                    StartDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2023, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    DescriptionMarkdown = "Built **ETL** pipeline with Python and Hadoop for retail analytics.",
                    Version = 1,
                    Tags = { new ProjectTag { TagId = tagId["Python"] }, new ProjectTag { TagId = tagId["Data Engineering"] } },
                },
                new()
                {
                    UserId = candidate1.Id, Title = "Sales Dashboard",
                    Location = "Remote",
                    StartDate = new DateTime(2023, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    DescriptionMarkdown = "Dashboard on **SQL** + Python for sales reporting.",
                    Version = 1,
                    Tags = { new ProjectTag { TagId = tagId["Python"] }, new ProjectTag { TagId = tagId["SQL"] } },
                },
                new()
                {
                    UserId = candidate1.Id, Title = "ML Churn Prediction",
                    Location = "Remote",
                    StartDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = null,
                    DescriptionMarkdown = "Churn prediction with **Machine Learning**.",
                    Version = 1,
                    Tags = { new ProjectTag { TagId = tagId["Machine Learning"] }, new ProjectTag { TagId = tagId["Python"] } },
                },
                new()
                {
                    UserId = candidate2.Id, Title = "CV Management Backend",
                    Location = "Berlin",
                    StartDate = new DateTime(2022, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DescriptionMarkdown = "Backend on **C#**, ASP.NET Core, EF Core, PostgreSQL.",
                    Version = 1,
                    Tags = { new ProjectTag { TagId = tagId["C#"] }, new ProjectTag { TagId = tagId["ASP.NET Core"] }, new ProjectTag { TagId = tagId["PostgreSQL"] } },
                },
                new()
                {
                    UserId = candidate2.Id, Title = "Dockerized API Gateway",
                    Location = "Berlin",
                    StartDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = null,
                    DescriptionMarkdown = "API gateway with **Docker** and PostgreSQL.",
                    Version = 1,
                    Tags = { new ProjectTag { TagId = tagId["Docker"] }, new ProjectTag { TagId = tagId["C#"] } },
                },
                new()
                {
                    UserId = candidate3.Id, Title = "Portfolio SPA",
                    Location = "Frisco",
                    StartDate = new DateTime(2023, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                    DescriptionMarkdown = "Portfolio **SPA** on React + TypeScript.",
                    Version = 1,
                    Tags = { new ProjectTag { TagId = tagId["React"] }, new ProjectTag { TagId = tagId["TypeScript"] } },
                },
                new()
                {
                    UserId = candidate3.Id, Title = "Team Dashboard UI",
                    Location = "Remote",
                    StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = null,
                    DescriptionMarkdown = "Dashboard UI with **React**, JavaScript.",
                    Version = 1,
                    Tags = { new ProjectTag { TagId = tagId["React"] }, new ProjectTag { TagId = tagId["JavaScript"] } },
                },
            };

            _context.Projects.AddRange(projects);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCvsLikesAndDiscussions()
    {
        var candidate1 = await _userManager.FindByEmailAsync("candidate1@curricuvita.com");
        var candidate2 = await _userManager.FindByEmailAsync("candidate2@curricuvita.com");
        var candidate3 = await _userManager.FindByEmailAsync("candidate3@curricuvita.com");
        var recruiter = await _userManager.FindByEmailAsync("recruiter@curricuvita.com");
        if (candidate1 == null || candidate2 == null || candidate3 == null || recruiter == null)
        {
            return;
        }

        var positions = await _context.Positions.ToDictionaryAsync(p => p.Title, p => p.Id);
        if (positions.Count == 0)
        {
            return;
        }

        var cvs = new List<CV>
        {
            new() { UserId = candidate1.Id, PositionId = positions["Junior Data Engineer"], Status = CvStatus.Published, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Version = 1 },
            new() { UserId = candidate1.Id, PositionId = positions["Data Scientist"], Status = CvStatus.Published, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Version = 1 },
            new() { UserId = candidate2.Id, PositionId = positions["Senior Backend .NET Developer"], Status = CvStatus.Published, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Version = 1 },
            new() { UserId = candidate2.Id, PositionId = positions["Generic Employee"], Status = CvStatus.Published, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Version = 1 },
            new() { UserId = candidate3.Id, PositionId = positions["Frontend React Developer"], Status = CvStatus.Published, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Version = 1 },
            new() { UserId = candidate3.Id, PositionId = positions["Generic Employee"], Status = CvStatus.Draft, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Version = 1 },
        };

        foreach (var cv in cvs)
        {
            if (!await _context.CVs.AnyAsync(c => c.UserId == cv.UserId && c.PositionId == cv.PositionId))
            {
                _context.CVs.Add(cv);
            }
        }
        await _context.SaveChangesAsync();

        var publishedCvs = await _context.CVs
            .Where(c => c.Status == CvStatus.Published)
            .Take(3)
            .ToListAsync();

        foreach (var cv in publishedCvs)
        {
            if (!await _context.Likes.AnyAsync(l => l.CVId == cv.Id && l.RecruiterId == recruiter.Id))
            {
                _context.Likes.Add(new Like
                {
                    CVId = cv.Id,
                    RecruiterId = recruiter.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        await _context.SaveChangesAsync();

        if (!await _context.DiscussionMessages.AnyAsync())
        {
            var messages = new List<DiscussionMessage>();
            foreach (var (title, id) in positions)
            {
                messages.Add(new DiscussionMessage
                {
                    PositionId = id,
                    AuthorId = recruiter.Id,
                    ContentMarkdown = $"Welcome to the **{title}** discussion! Please keep it professional.",
                    CreatedAt = DateTime.UtcNow.AddHours(-5),
                });
            }

            messages.Add(new DiscussionMessage
            {
                PositionId = positions["Junior Data Engineer"],
                AuthorId = candidate1.Id,
                ContentMarkdown = "Hello! Is **Hadoop** strictly required for this role?",
                CreatedAt = DateTime.UtcNow.AddHours(-2),
            });
            messages.Add(new DiscussionMessage
            {
                PositionId = positions["Junior Data Engineer"],
                AuthorId = recruiter.Id,
                ContentMarkdown = "Hadoop is optional — **Python** and English matter most.",
                CreatedAt = DateTime.UtcNow.AddHours(-1),
            });

            _context.DiscussionMessages.AddRange(messages);
            await _context.SaveChangesAsync();
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
                var result = await _roleManager.CreateAsync(new IdentityRole<int>(role));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to create role '{role}': {DescribeErrors(result)}");
                }
            }
        }
    }

    private static string DescribeErrors(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
}
