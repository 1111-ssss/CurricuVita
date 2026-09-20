using Ardalis.Specification;
using Domain.Contracts.UserContracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Users.GetPublicProfile;

public class GetPublicProfileHandler : IRequestHandler<GetPublicProfileQuery, Result<PublicProfileDto>>
{
    private readonly IUserRepository _users;
    private readonly IRepositoryBase<UserAttributeValue> _values;
    private readonly IRepositoryBase<CV> _cvs;

    public GetPublicProfileHandler(
        IUserRepository users,
        IRepositoryBase<UserAttributeValue> values,
        IRepositoryBase<CV> cvs
    )
    {
        _users = users;
        _values = values;
        _cvs = cvs;
    }

    public async Task<Result<PublicProfileDto>> Handle(GetPublicProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result<PublicProfileDto>.Failure(Errors.UserNotFound);
        }

        var values = await _values.ListAsync(
            new UserAttributeValuesByUserSpec(request.UserId), cancellationToken
        );
        var cvs = await _cvs.ListAsync(new CvsByUserSpec(request.UserId), cancellationToken);

        var visible = cvs
            .Where(c => c.Status == CvStatus.Published && PositionAccessEvaluator.HasAccess(c.Position, values))
            .Select(c => new PublicProfileCvDto(
                c.Id,
                c.PositionId,
                c.Position.Title,
                c.UpdatedAt,
                c.Likes.Count
            ))
            .ToList();

        var name = $"{user.FirstName} {user.LastName}".Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            name = user.Email ?? $"User {user.Id}";
        }

        return Result<PublicProfileDto>.Success(new PublicProfileDto(
            user.Id, name, user.Location ?? string.Empty, user.AvatarUrl, visible
        ));
    }
}
