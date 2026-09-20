using Domain.Contracts.PositionContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
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

        var items = await _positions.SearchPositionsAsync(
            request.TitlePrefix,
            request.OnlyPublic,
            skip,
            take,
            request.SearchText,
            request.Company,
            request.Level,
            cancellationToken
        );

        return Result<List<PositionListItemDto>>.Success(
            items.Select(p => p.ToDto()).ToList()
        );
    }
}
