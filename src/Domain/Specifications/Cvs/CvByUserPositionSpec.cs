using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Cvs;

public sealed class CvByUserPositionSpec : SingleResultSpecification<CV>
{
    public CvByUserPositionSpec(int userId, int positionId)
    {
        Query.Where(c => c.UserId == userId && c.PositionId == positionId);
    }
}
