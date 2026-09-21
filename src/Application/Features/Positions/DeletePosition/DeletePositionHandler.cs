using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.DeletePosition;

public class DeletePositionHandler : IRequestHandler<DeletePositionCommand, Result>
{
    private readonly IPositionRepository _positions;

    public DeletePositionHandler(IPositionRepository positions)
    {
        _positions = positions;
    }

    public async Task<Result> Handle(
        DeletePositionCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _positions.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(Errors.PositionNotFound);
        }

        await _positions.DeleteAsync(entity, cancellationToken);
        await _positions.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
