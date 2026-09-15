using Domain.Contracts.AttributeContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.SaveAttributeValues;

public record SaveAttributeValuesCommand(
    int UserId,
    List<AttributeValueInput> Items,
    int RequesterUserId,
    bool IsAdmin = false
) : IRequest<Result<List<ProfileAttributeValueDto>>>;
