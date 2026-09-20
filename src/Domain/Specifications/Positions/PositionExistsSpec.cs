using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Positions;

public sealed class PositionExistsSpec : Specification<Position>
{
    public PositionExistsSpec(int id)
    {
        Query
            .Where(p => p.Id == id);
    }
}
