using Ardalis.Specification;
using Domain.Entities;
using Domain.Helpers;

namespace Domain.Specifications.Profile;

public sealed class AttachableAttributesSpec : Specification<AttributeDefinition>
{
    public AttachableAttributesSpec()
    {
        Query
            .Where(a => a.Category != AttributeOptionsHelper.ProtectedCategory)
            .OrderBy(a => a.Category)
            .ThenBy(a => a.Name);
    }
}
