using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Profile;

public sealed class ProjectsByUserSpec : Specification<Project>
{
    public ProjectsByUserSpec(int userId)
    {
        Query
            .Where(p => p.UserId == userId)
            .Include(p => p.Tags)
            .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.StartDate)
            .ThenByDescending(p => p.Id);
    }
}
