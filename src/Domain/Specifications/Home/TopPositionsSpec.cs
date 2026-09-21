using Ardalis.Specification;
using Domain.Contracts.HomeContracts;
using Domain.Entities;

namespace Domain.Specifications.Home;

public sealed class TopPositionsSpec : Specification<Position, HomePositionItemDto>
{
    public TopPositionsSpec(int take = 5)
    {
        Query.OrderByDescending(p => p.CVs.Count)
            .ThenByDescending(p => p.UpdatedAt)
            .Take(take);

        Query.Select(p => new HomePositionItemDto(
            p.Id, p.Title, p.Company, p.Level, p.IsPublic, p.CreatedAt, p.UpdatedAt, p.CVs.Count));
    }
}
