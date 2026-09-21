using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.GetPublicProfile;

public record GetPublicProfileQuery(
    int UserId
) : IRequest<Result<PublicProfileDto>>;
