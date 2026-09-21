using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Attributes;

public sealed class AttributeByIdSpec : SingleResultSpecification<AttributeDefinition, AttributeProjection>
{
    public AttributeByIdSpec(int id)
    {
        Query.Where(a => a.Id == id);

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
