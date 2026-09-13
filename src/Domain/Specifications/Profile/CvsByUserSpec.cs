using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Profile;

public sealed class CvsByUserSpec : Specification<CV>
{
    public CvsByUserSpec(int userId)
    {
        Query
            .Where(c => c.UserId == userId)
            .Include(c => c.Position)
            .OrderByDescending(c => c.UpdatedAt);
    }
}
