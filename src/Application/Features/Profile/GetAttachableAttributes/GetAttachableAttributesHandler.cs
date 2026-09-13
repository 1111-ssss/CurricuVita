using AutoMapper;
using Domain.Contracts.AttributeContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Profile.GetAttachableAttributes;

public class GetAttachableAttributesHandler : IRequestHandler<GetAttachableAttributesQuery, Result<List<AttributeDto>>>
{
    private readonly IAttributeRepository _attributes;
    private readonly IMapper _mapper;

    public GetAttachableAttributesHandler(
        IAttributeRepository attributes,
        IMapper mapper
    )
    {
        _attributes = attributes;
        _mapper = mapper;
    }

    public async Task<Result<List<AttributeDto>>> Handle(GetAttachableAttributesQuery request, CancellationToken cancellationToken)
    {
        var items = await _attributes.ListAsync(new AttachableAttributesSpec(), cancellationToken);

        return Result<List<AttributeDto>>.Success(
            _mapper.Map<List<AttributeDto>>(items)
        );
    }
}
