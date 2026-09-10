using Domain.Contracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using MediatR;

namespace Application.Features.Attributes.GetAttribute;

public class GetAttributeHandler : IRequestHandler<GetAttributeQuery, Result<AttributeDto>>
{
    private readonly IAttributeRepository _attributes;

    public GetAttributeHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result<AttributeDto>> Handle(
        GetAttributeQuery request,
        CancellationToken cancellationToken)
    {
        var item = await _attributes.SingleOrDefaultAsync(
            new AttributeByIdSpec(request.Id),
            cancellationToken);

        if (item is null)
        {
            return Result<AttributeDto>.Failure(Errors.AttributeNotFound);
        }

        return Result<AttributeDto>.Success(
            item.ToDto()
        );
    }
}
