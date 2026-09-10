using Domain.Contracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using MediatR;

namespace Application.Features.Attributes.ListAttributes;

public class ListAttributesHandler : IRequestHandler<ListAttributesQuery, Result<List<AttributeDto>>>
{
    private readonly IAttributeRepository _attributes;

    public ListAttributesHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result<List<AttributeDto>>> Handle(
        ListAttributesQuery request,
        CancellationToken cancellationToken
    )
    {
        var take = Math.Clamp(request.Take, 1, 200);
        var skip = Math.Max(request.Skip, 0);

        var items = await _attributes.ListAsync(
            new AttributeSearchSpec(request.NamePrefix, request.Category, skip, take),
            cancellationToken
        );

        return Result<List<AttributeDto>>.Success(
            items.Select(p => p.ToDto())
                .ToList()
        );
    }
}
