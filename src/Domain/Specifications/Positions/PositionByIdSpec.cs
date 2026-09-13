using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Positions;

public sealed class PositionByIdSpec : SingleResultSpecification<Position>
{
    public PositionByIdSpec(int id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.RequiredAttributes)
                .ThenInclude(pa => pa.AttributeDefinition)
            .Include(p => p.AccessRules)
                .ThenInclude(r => r.AttributeDefinition)
            .Include(p => p.RequiredTags)
                .ThenInclude(rt => rt.Tag);
    }
}
