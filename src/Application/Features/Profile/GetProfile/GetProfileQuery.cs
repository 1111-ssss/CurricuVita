using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.GetProfile;

public record GetProfileQuery(
    int UserId
) : IRequest<Result<ProfileDto>>;
