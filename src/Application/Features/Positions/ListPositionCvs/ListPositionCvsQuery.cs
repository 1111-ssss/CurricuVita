using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.ListPositionCvs;

public record ListPositionCvsQuery(
    int PositionId,
    string? SearchText = null
) : IRequest<Result<List<PositionCvListItemDto>>>;
