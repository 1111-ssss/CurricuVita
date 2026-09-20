using Ardalis.Specification;
using Domain.Contracts.HomeContracts;
using Domain.Entities;

namespace Domain.Specifications.Home;

public sealed class TagCloudSpec : Specification<Tag, TagCloudItemDto>
{
    public TagCloudSpec(int take = 50)
    {
        Query.Where(t => t.ProjectTags.Count + t.PositionTags.Count > 0);

        Query.OrderByDescending(t => t.ProjectTags.Count + t.PositionTags.Count)
            .Take(take);

        Query.Select(t => new TagCloudItemDto(
            t.Name,
            t.ProjectTags.Count + t.PositionTags.Count));
    }
}
