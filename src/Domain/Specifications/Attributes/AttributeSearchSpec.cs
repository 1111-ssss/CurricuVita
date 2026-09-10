using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Attributes;

public sealed class AttributeSearchSpec : Specification<AttributeDefinition, AttributeProjection>
{
    public AttributeSearchSpec(string? namePrefix, string? category, int skip, int take)
    {
        if (!string.IsNullOrWhiteSpace(namePrefix))
        {
            Query.Where(a => a.Name.StartsWith(namePrefix));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            Query.Where(a => a.Category == category);
        }

        Query.OrderBy(a => a.Name)
            .Skip(skip)
            .Take(take);

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
