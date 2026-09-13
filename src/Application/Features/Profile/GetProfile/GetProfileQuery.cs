using Domain.Contracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.GetProfile;

public record GetProfileQuery(
    int UserId
) : IRequest<Result<ProfileDto>>;
