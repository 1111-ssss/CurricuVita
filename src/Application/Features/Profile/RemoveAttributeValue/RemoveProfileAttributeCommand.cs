using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.RemoveAttributeValue;

public record RemoveProfileAttributeCommand(
    int UserId,
    int ValueId
) : IRequest<Result>;
