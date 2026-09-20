using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Tags;

public sealed class TagByNameSpec : SingleResultSpecification<Tag>
{
    public TagByNameSpec(string name)
    {
        Query
            .Where(t => t.Name == name);
    }
}
