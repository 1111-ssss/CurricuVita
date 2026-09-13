using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Positions;

public sealed class PositionCvsSpec : Specification<CV>
{
    public PositionCvsSpec(int positionId)
    {
        Query
            .Where(c => c.PositionId == positionId)
            .Include(c => c.User)
            .Include(c => c.Likes)
            .OrderByDescending(c => c.UpdatedAt);
    }
}
