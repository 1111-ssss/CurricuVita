using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.DeleteAttribute;

public record DeleteAttributeCommand(
    int Id
) : IRequest<Result>;
