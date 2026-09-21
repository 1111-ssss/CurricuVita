using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.GetPosition;

public record GetPositionQuery(
    int Id
) : IRequest<Result<PositionDetailDto>>;
