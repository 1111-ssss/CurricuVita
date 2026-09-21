using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.DeleteAttribute;

public record DeleteAttributesBulkCommand(
    List<int> Ids
) : IRequest<Result<int>>;
