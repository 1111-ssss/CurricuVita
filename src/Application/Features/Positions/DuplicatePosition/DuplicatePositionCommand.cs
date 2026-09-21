using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.DuplicatePosition;

public record DuplicatePositionCommand(
    int SourcePositionId
) : IRequest<Result<PositionDetailDto>>;
