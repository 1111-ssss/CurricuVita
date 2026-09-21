using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Attributes;

public sealed class AttributeCategoriesSpec : Specification<AttributeDefinition, string>
{
    public AttributeCategoriesSpec()
    {
        Query.OrderBy(a => a.Category);
        Query.Select(a => a.Category);
    }
}
