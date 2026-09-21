using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Attributes;

public sealed class AttributeNameExistsSpec : Specification<AttributeDefinition>
{
    public AttributeNameExistsSpec(string name, int? excludeId = null)
    {
        Query.Where(a => a.Name == name);

        if (excludeId.HasValue)
        {
            Query.Where(a => a.Id != excludeId.Value);
        }
    }
}
