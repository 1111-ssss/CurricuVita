using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.GetPositionAggregates;

public record GetPositionAggregatesQuery(
    int PositionId
) : IRequest<Result<PositionAggregatesDto>>;
