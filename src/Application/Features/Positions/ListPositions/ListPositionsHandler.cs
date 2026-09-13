using Domain.Contracts.PositionContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Positions.ListPositions;

public class ListPositionsHandler : IRequestHandler<ListPositionsQuery, Result<List<PositionListItemDto>>>
{
    private readonly IPositionRepository _positions;

    public ListPositionsHandler(IPositionRepository positions)
    {
        _positions = positions;
    }

    public async Task<Result<List<PositionListItemDto>>> Handle(
        ListPositionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var take = Math.Clamp(request.Take, 1, 200);
        var skip = Math.Max(request.Skip, 0);

        var items = await _positions.ListAsync(
            new PositionSearchSpec(request.TitlePrefix, request.OnlyPublic, skip, take),
            cancellationToken
        );

        return Result<List<PositionListItemDto>>.Success(
            items.Select(p => p.ToDto()).ToList()
        );
    }
}
