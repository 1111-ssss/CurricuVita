using Domain.Contracts.AttributeContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.GetAttachableAttributes;

public record GetAttachableAttributesQuery : IRequest<Result<List<AttributeDto>>>;
