using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Attributes;

public sealed class AttributesByIdsSpec : Specification<AttributeDefinition>
{
    public AttributesByIdsSpec(List<int> ids)
    {
        Query
            .Where(a => ids.Contains(a.Id));
    }
}
