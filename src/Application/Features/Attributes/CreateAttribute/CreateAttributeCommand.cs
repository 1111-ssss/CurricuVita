using Domain.Contracts.AttributeContracts;
using Domain.Enums;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.CreateAttribute;

public record CreateAttributeCommand(
    string Category,
    string Name,
    string Description,
    AttributeDataType DataType,
    List<string>? Options = null
) : IRequest<Result<AttributeDto>>;
