using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Attributes;

public sealed class AttributeRecentlyUsedSpec : Specification<AttributeDefinition, AttributeProjection>
{
    public AttributeRecentlyUsedSpec(int take)
    {
        Query.OrderByDescending(a => a.PositionAttributes.Count + a.UserValues.Count)
            .ThenByDescending(a => a.CreatedAt);

        Query.Take(take);

        Query.Select(a => new AttributeProjection(
            a.Id,
            a.Category,
            a.Name,
            a.Description,
            a.DataType,
            a.OptionsJson,
            a.Version,
            a.CreatedAt,
            a.PositionAttributes.Count + a.UserValues.Count
        ));
    }
}
