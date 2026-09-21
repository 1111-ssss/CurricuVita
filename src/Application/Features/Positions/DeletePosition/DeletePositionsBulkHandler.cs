using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.DeletePosition;

public class DeletePositionsBulkHandler : IRequestHandler<DeletePositionsBulkCommand, Result<int>>
{
    private readonly IPositionRepository _positions;

    public DeletePositionsBulkHandler(IPositionRepository positions)
    {
        _positions = positions;
    }

    public async Task<Result<int>> Handle(
        DeletePositionsBulkCommand request,
        CancellationToken cancellationToken
    )
    {
        var ids = request.Ids?.Distinct().ToList() ?? new List<int>();
        if (ids.Count == 0)
        {
            return Result<int>.Success(0);
        }

        var deleted = await _positions.DeletePositionsBulkAsync(ids, cancellationToken);
        return Result<int>.Success(deleted);
    }
}
