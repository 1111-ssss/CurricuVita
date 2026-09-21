using Domain.Entities;
using Domain.Enums;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests;

[Collection(PostgresCollection.Name)]
public sealed class PositionRepositoryTests
{
    private readonly PostgresFixture _fx;

    public PositionRepositoryTests(PostgresFixture fx) => _fx = fx;

    [Fact]
    public async Task Migrate_AppliesCleanly_AndCreatesFtsIndexes()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();

        var indexes = await ctx.Database
            .SqlQueryRaw<string>("SELECT indexname FROM pg_indexes WHERE schemaname = 'public'")
            .ToListAsync();

        Assert.Contains("IX_Positions_SearchVector", indexes);
        Assert.Contains("IX_Users_SearchVector", indexes);
    }

    [Fact]
    public async Task SearchPositions_FtsFindsByWordForm_LikeWouldMiss()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionsAsync(null, null, 0, 50, "developers");

        Assert.Single(result);
        Assert.Equal("Senior Backend Developer", result[0].Title);
    }

    [Fact]
    public async Task SearchPositions_LikeFallbackFindsSubstring_FtsMisses()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionsAsync(null, null, 0, 50, "velop");

        Assert.Single(result);
        Assert.Equal("Senior Backend Developer", result[0].Title);
    }

    [Fact]
    public async Task SearchPositions_FindsByTagName()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionsAsync(null, null, 0, 50, "golang");

        Assert.Single(result);
        Assert.Equal("Senior Backend Developer", result[0].Title);
        Assert.Contains("golang", result[0].Tags);
    }

    [Fact]
    public async Task SearchPositions_AppliesFilters_Pagination_AndProjection()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var page = await repo.SearchPositionsAsync(null, true, 0, 50);

        Assert.Single(page);
        var item = page[0];
        Assert.Equal("Senior Backend Developer", item.Title);
        Assert.Equal("Acme", item.Company);
        Assert.Equal("Senior", item.Level);
        Assert.True(item.IsPublic);
        Assert.Equal(2, item.CvCount);
        Assert.Equal(2, item.Tags.Count);

        var empty = await repo.SearchPositionsAsync(null, true, 1, 50);
        Assert.Empty(empty);
    }

    [Fact]
    public async Task SearchPositions_CompanyFilter_IsCaseInsensitive()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionsAsync(null, null, 0, 50, null, "ACME");

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchPositions_LevelFilter_IsExact()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var senior = await repo.SearchPositionsAsync(null, null, 0, 50, null, null, "Senior");
        Assert.Single(senior);

        var seniorLower = await repo.SearchPositionsAsync(null, null, 0, 50, null, null, "senior");
        Assert.Empty(seniorLower);
    }

    [Fact]
    public async Task SearchPositions_BlankSearchText_ReturnsAll()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionsAsync(null, null, 0, 50, "   ");

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchPositionCvs_FindsByLastName_ExcludesDrafts()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        var ids = await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionCvsAsync(ids.BackendId, "pushkin");

        var single = Assert.Single(result);
        Assert.NotNull(single.User);
        Assert.NotNull(single.Likes);
        Assert.Equal("Alexander", single.User.FirstName);
    }

    [Fact]
    public async Task SearchPositionCvs_LikeFallbackFindsNamePrefix()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        var ids = await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionCvsAsync(ids.BackendId, "pushk");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchPositionCvs_OtherPosition_ReturnsEmpty()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        var ids = await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var result = await repo.SearchPositionCvsAsync(ids.FrontendId, "pushkin");

        Assert.Empty(result);
    }

    [Fact]
    public async Task TrySaveWithConcurrency_StaleVersion_ReturnsFalse()
    {
        await _fx.ResetAsync();
        await using var ctx = _fx.CreateContext();
        var ids = await SeedPositionsAsync(ctx);
        var repo = new PositionRepository(ctx);

        var position = await repo.GetByIdAsync(ids.BackendId);
        Assert.NotNull(position);

        position.Title = "Senior Backend Developer (updated)";
        Assert.True(await repo.TrySaveWithConcurrencyAsync(position, expectedVersion: 1));
        Assert.Equal(2, position.Version);

        position.Title = "Stale write";
        Assert.False(await repo.TrySaveWithConcurrencyAsync(position, expectedVersion: 1));
    }

    private sealed record SeedIds(int BackendId, int FrontendId);

    private static async Task<SeedIds> SeedPositionsAsync(AppDbContext ctx)
    {
        var now = DateTime.UtcNow;

        var creator = new User
        {
            UserName = "creator@example.com",
            Email = "creator@example.com",
            FirstName = "Ivan",
            LastName = "Creator",
            CreatedAt = now,
            UpdatedAt = now,
        };
        var alice = new User
        {
            UserName = "pushkin@example.com",
            Email = "pushkin@example.com",
            FirstName = "Alexander",
            LastName = "Pushkin",
            CreatedAt = now,
            UpdatedAt = now,
        };
        var bob = new User
        {
            UserName = "bob@example.com",
            Email = "bob@example.com",
            FirstName = "Bob",
            LastName = "Builder",
            CreatedAt = now,
            UpdatedAt = now,
        };
        ctx.Users.AddRange(creator, alice, bob);
        await ctx.SaveChangesAsync();

        var golang = new Tag { Name = "golang" };
        var postgres = new Tag { Name = "postgres" };
        var react = new Tag { Name = "react" };
        ctx.Tags.AddRange(golang, postgres, react);
        await ctx.SaveChangesAsync();

        var backend = new Position
        {
            Title = "Senior Backend Developer",
            DescriptionMarkdown = "We need an experienced backend developer for distributed systems.",
            Company = "Acme",
            Level = "Senior",
            IsPublic = true,
            CreatedById = creator.Id,
            CreatedAt = now.AddDays(-2),
            UpdatedAt = now,
            Version = 1,
            RequiredTags = new List<PositionTag>
            {
                new() { TagId = golang.Id },
                new() { TagId = postgres.Id },
            },
        };
        var frontend = new Position
        {
            Title = "Frontend Engineer",
            DescriptionMarkdown = "React single-page applications.",
            Company = "Acme",
            Level = "Middle",
            IsPublic = false,
            CreatedById = creator.Id,
            CreatedAt = now.AddDays(-3),
            UpdatedAt = now.AddDays(-1),
            Version = 1,
            RequiredTags = new List<PositionTag>
            {
                new() { TagId = react.Id },
            },
        };
        ctx.Positions.AddRange(backend, frontend);
        await ctx.SaveChangesAsync();

        var aliceCv = new CV
        {
            UserId = alice.Id,
            PositionId = backend.Id,
            Status = CvStatus.Published,
            CreatedAt = now,
            UpdatedAt = now,
            Version = 1,
        };
        var bobCv = new CV
        {
            UserId = bob.Id,
            PositionId = backend.Id,
            Status = CvStatus.Published,
            CreatedAt = now,
            UpdatedAt = now.AddMinutes(-5),
            Version = 1,
        };
        var aliceDraft = new CV
        {
            UserId = alice.Id,
            PositionId = frontend.Id,
            Status = CvStatus.Draft,
            CreatedAt = now,
            UpdatedAt = now,
            Version = 1,
        };
        ctx.CVs.AddRange(aliceCv, bobCv, aliceDraft);
        await ctx.SaveChangesAsync();

        ctx.Likes.Add(new Like
        {
            CVId = aliceCv.Id,
            RecruiterId = creator.Id,
            CreatedAt = now,
        });
        await ctx.SaveChangesAsync();

        return new SeedIds(backend.Id, frontend.Id);
    }
}
