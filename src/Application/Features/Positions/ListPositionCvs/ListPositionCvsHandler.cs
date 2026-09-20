using Domain.Contracts.PositionContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Positions.ListPositionCvs;

public class ListPositionCvsHandler : IRequestHandler<ListPositionCvsQuery, Result<List<PositionCvListItemDto>>>
{
    private readonly IPositionRepository _positions;

    public ListPositionCvsHandler(IPositionRepository positions)
    {
        _positions = positions;
    }

    public async Task<Result<List<PositionCvListItemDto>>> Handle(
        ListPositionCvsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!await _positions.AnyAsync(new PositionExistsSpec(request.PositionId), cancellationToken))
        {
            return Result<List<PositionCvListItemDto>>.Failure(Errors.PositionNotFound);
        }

        var cvs = await _positions.SearchPositionCvsAsync(request.PositionId, request.SearchText, cancellationToken);

        return Result<List<PositionCvListItemDto>>.Success(
            cvs.Select(c => new PositionCvListItemDto(
                c.Id,
                c.UserId,
                $"{c.User?.FirstName} {c.User?.LastName}".Trim(),
                c.Status,
                c.CreatedAt,
                c.UpdatedAt,
                c.Likes.Count
            )).ToList()
        );
    }
}
