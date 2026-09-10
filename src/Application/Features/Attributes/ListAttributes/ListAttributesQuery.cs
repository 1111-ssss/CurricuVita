using Domain.Contracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.ListAttributes;

public record ListAttributesQuery(
    string? NamePrefix = null,
    string? Category = null,
    int Take = 50,
    int Skip = 0
) : IRequest<Result<List<AttributeDto>>>;
