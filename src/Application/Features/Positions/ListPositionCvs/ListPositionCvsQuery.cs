using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.ListPositionCvs;

public record ListPositionCvsQuery(
    int PositionId,
    string? SearchText = null,
    int RequesterUserId = 0,
    bool IsRecruiter = false,
    bool IsAdmin = false
) : IRequest<Result<List<PositionCvListItemDto>>>;
