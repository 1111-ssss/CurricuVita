using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.DeletePosition;

public record DeletePositionsBulkCommand(
    List<int> Ids
) : IRequest<Result<int>>;
