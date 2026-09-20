using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Positions;

public sealed class PositionSearchSpec : Specification<Position, PositionListProjection>
{
    public PositionSearchSpec(
        string? titlePrefix,
        bool? onlyPublic,
        int skip,
        int take,
        string? company = null,
        string? level = null
    )
    {
        if (!string.IsNullOrWhiteSpace(titlePrefix))
        {
            Query.Where(p => p.Title.StartsWith(titlePrefix));
        }

        if (!string.IsNullOrWhiteSpace(company))
        {
            var c = company.Trim().ToLower();
            Query.Where(p => p.Company != null && p.Company.ToLower().Contains(c));
        }

        if (!string.IsNullOrWhiteSpace(level))
        {
            var l = level.Trim();
            Query.Where(p => p.Level == l);
        }

        if (onlyPublic == true)
        {
            Query.Where(p => p.IsPublic);
        }

        Query.OrderByDescending(p => p.UpdatedAt)
            .Skip(skip)
            .Take(take);

        Query.Select(p => new PositionListProjection(
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
        ));
    }
}
