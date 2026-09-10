using Domain.Contracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.RecentlyUsed;

public record GetRecentlyUsedQuery(
    int Take = 10
) : IRequest<Result<List<AttributeDto>>>;
