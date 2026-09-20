using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.ListPositions;

public record ListPositionsQuery(
    string? TitlePrefix = null,
    bool? OnlyPublic = null,
    int Take = 50,
    int Skip = 0,
    string? SearchText = null,
    string? Company = null,
    string? Level = null
) : IRequest<Result<List<PositionListItemDto>>>;
