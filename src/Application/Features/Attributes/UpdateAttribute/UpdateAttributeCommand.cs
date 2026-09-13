using Domain.Contracts.AttributeContracts;
using Domain.Enums;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.UpdateAttribute;

public record UpdateAttributeCommand(
    int Id,
    string Category,
    string Name,
    string Description,
    AttributeDataType DataType,
    List<string>? Options,
    int Version
) : IRequest<Result<AttributeDto>>;
