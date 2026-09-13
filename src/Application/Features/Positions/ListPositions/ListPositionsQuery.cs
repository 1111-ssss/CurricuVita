using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.ListPositions;

public record ListPositionsQuery(
    string? TitlePrefix = null,
    bool? OnlyPublic = null,
    int Take = 50,
    int Skip = 0
) : IRequest<Result<List<PositionListItemDto>>>;
