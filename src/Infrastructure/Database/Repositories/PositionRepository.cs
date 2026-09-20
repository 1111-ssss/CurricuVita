using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Database;
using Domain.Specifications.Positions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class PositionRepository : BaseRepository<Position>, IPositionRepository
{
    private const string FtsConfig = "english";

    private readonly AppDbContext _context;

    public PositionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> TrySaveWithConcurrencyAsync(
        Position entity,
        int expectedVersion,
        CancellationToken cancellationToken = default
    )
    {
        _context.Entry(entity).Property(e => e.Version).OriginalValue = expectedVersion;
        entity.Version = expectedVersion + 1;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<List<PositionListProjection>> SearchPositionsAsync(
        string? titlePrefix,
        bool? onlyPublic,
        int skip,
        int take,
        string? searchText = null,
        string? company = null,
        string? level = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.Positions.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(titlePrefix))
        {
            query = query.Where(p => p.Title.StartsWith(titlePrefix));
        }

        if (!string.IsNullOrWhiteSpace(company))
        {
            var c = company.Trim().ToLower();
            query = query.Where(p => p.Company != null && p.Company.ToLower().Contains(c));
        }

        if (!string.IsNullOrWhiteSpace(level))
        {
            var l = level.Trim();
            query = query.Where(p => p.Level == l);
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var raw = searchText.Trim();
            var q = raw.ToLower();

            query = query.Where(p =>
                EF.Functions.ToTsVector(FtsConfig,
                        (p.Title ?? string.Empty) + " " +
                        (p.DescriptionMarkdown ?? string.Empty) + " " +
                        (p.Company ?? string.Empty))
                    .Matches(EF.Functions.PlainToTsQuery(FtsConfig, raw)) ||
                (p.Title != null && p.Title.ToLower().Contains(q)) ||
                (p.DescriptionMarkdown != null && p.DescriptionMarkdown.ToLower().Contains(q)) ||
                (p.Company != null && p.Company.ToLower().Contains(q)) ||
                p.RequiredTags.Any(rt => rt.Tag.Name.ToLower().Contains(q)));
        }

        if (onlyPublic == true)
        {
            query = query.Where(p => p.IsPublic);
        }

        return await query
            .OrderByDescending(p => p.UpdatedAt)
            .Skip(skip)
            .Take(take)
            .Select(p => new PositionListProjection(
                p.Id,
                p.Title,
                p.Company,
                p.Level,
                p.IsPublic,
                p.CreatedAt,
                p.UpdatedAt,
                p.RequiredAttributes.Count,
                p.CVs.Count,
                p.RequiredTags.Select(rt => rt.Tag.Name).ToList()
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CV>> SearchPositionCvsAsync(
        int positionId,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.CVs
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.Likes)
            .Where(c => c.PositionId == positionId && c.Status == CvStatus.Published);

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var raw = searchText.Trim();
            var q = raw.ToLower();

            query = query.Where(c =>
                EF.Functions.ToTsVector(FtsConfig,
                        (c.User.FirstName ?? string.Empty) + " " +
                        (c.User.LastName ?? string.Empty) + " " +
                        (c.User.Email ?? string.Empty))
                    .Matches(EF.Functions.PlainToTsQuery(FtsConfig, raw)) ||
                (c.User.FirstName != null && c.User.FirstName.ToLower().Contains(q)) ||
                (c.User.LastName != null && c.User.LastName.ToLower().Contains(q)) ||
                (c.User.Email != null && c.User.Email.ToLower().Contains(q)));
        }

        return await query
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync(cancellationToken);
    }
}
