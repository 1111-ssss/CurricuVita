using Ardalis.Specification;
using Domain.Contracts.HomeContracts;
using Domain.Entities;

namespace Domain.Specifications.Home;

public sealed class LatestPositionsSpec : Specification<Position, HomePositionItemDto>
{
    public LatestPositionsSpec(int take = 10)
    {
        Query.OrderByDescending(p => p.CreatedAt)
            .Take(take);

        Query.Select(p => new HomePositionItemDto(
            p.Id, p.Title, p.IsPublic, p.CreatedAt, p.UpdatedAt, p.CVs.Count));
    }
}
