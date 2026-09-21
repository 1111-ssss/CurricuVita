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
            var c = company.Trim();
            query = query.Where(p => p.Company != null && EF.Functions.ILike(p.Company, $"%{c}%"));
        }

        if (!string.IsNullOrWhiteSpace(level))
        {
            var l = level.Trim();
            query = query.Where(p => p.Level == l);
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var raw = searchText.Trim();

            query = query.Where(p =>
                EF.Functions.ToTsVector(FtsConfig,
                        (p.Title ?? string.Empty) + " " +
                        (p.DescriptionMarkdown ?? string.Empty) + " " +
                        (p.Company ?? string.Empty))
                    .Matches(EF.Functions.PlainToTsQuery(FtsConfig, raw)) ||
                (p.Title != null && EF.Functions.ILike(p.Title, $"%{raw}%")) ||
                (p.DescriptionMarkdown != null && EF.Functions.ILike(p.DescriptionMarkdown, $"%{raw}%")) ||
                (p.Company != null && EF.Functions.ILike(p.Company, $"%{raw}%")) ||
                p.RequiredTags.Any(rt => EF.Functions.ILike(rt.Tag.Name, $"%{raw}%")));
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

            query = query.Where(c =>
                EF.Functions.ToTsVector(FtsConfig,
                        (c.User.FirstName ?? string.Empty) + " " +
                        (c.User.LastName ?? string.Empty) + " " +
                        (c.User.Email ?? string.Empty))
                    .Matches(EF.Functions.PlainToTsQuery(FtsConfig, raw)) ||
                (c.User.FirstName != null && EF.Functions.ILike(c.User.FirstName, $"%{raw}%")) ||
                (c.User.LastName != null && EF.Functions.ILike(c.User.LastName, $"%{raw}%")) ||
                (c.User.Email != null && EF.Functions.ILike(c.User.Email, $"%{raw}%")));
        }

        return await query
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> DeletePositionsBulkAsync(
        ICollection<int> ids,
        CancellationToken cancellationToken = default
    )
    {
        if (ids is null || ids.Count == 0)
        {
            return 0;
        }

        return await _context.Positions
            .Where(p => ids.Contains(p.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }
}
