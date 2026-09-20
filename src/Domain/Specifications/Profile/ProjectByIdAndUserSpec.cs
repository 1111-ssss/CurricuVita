using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Profile;

public sealed class ProjectByIdAndUserSpec : SingleResultSpecification<Project>
{
    public ProjectByIdAndUserSpec(int projectId, int userId)
    {
        Query
            .Where(p => p.Id == projectId && p.UserId == userId)
            .Include(p => p.Tags)
            .ThenInclude(pt => pt.Tag);
    }
}
