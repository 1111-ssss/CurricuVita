using Domain.Contracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.GetAttribute;

public record GetAttributeQuery(
    int Id
) : IRequest<Result<AttributeDto>>;
