using Domain.Contracts.AttributeContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using MediatR;

namespace Application.Features.Attributes.RecentlyUsed;

public class GetRecentlyUsedHandler : IRequestHandler<GetRecentlyUsedQuery, Result<List<AttributeDto>>>
{
    private readonly IAttributeRepository _attributes;

    public GetRecentlyUsedHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result<List<AttributeDto>>> Handle(
        GetRecentlyUsedQuery request,
        CancellationToken cancellationToken
    )
    {
        var take = Math.Clamp(request.Take, 1, 50);

        var items = await _attributes.ListAsync(
            new AttributeRecentlyUsedSpec(take),
            cancellationToken);

        return Result<List<AttributeDto>>.Success(
            items.Select(p => p.ToDto())
                .ToList()
        );
    }
}
