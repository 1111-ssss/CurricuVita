using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.UpdateMe;

public record UpdateMeCommand(
    int UserId,
    string FirstName,
    string LastName,
    string Location,
    string? AvatarUrl,
    string? AvatarPublicId,
    int Version
) : IRequest<Result<MeDto>>;
