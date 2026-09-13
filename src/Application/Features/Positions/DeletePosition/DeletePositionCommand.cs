using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.DeletePosition;

public record DeletePositionCommand(
    int Id
) : IRequest<Result>;
