using Ardalis.Specification;
using Domain.Constants;
using Domain.Contracts.HomeContracts;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using Domain.Specifications.Home;
using MediatR;

namespace Application.Features.Home.GetDashboard;

public class GetHomeDashboardHandler : IRequestHandler<GetHomeDashboardQuery, Result<HomeDashboardDto>>
{
    private readonly IPositionRepository _positions;
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IRepositoryBase<Tag> _tags;
    private readonly IUserRepository _users;

    public GetHomeDashboardHandler(
        IPositionRepository positions,
        IRepositoryBase<CV> cvs,
        IRepositoryBase<Tag> tags,
        IUserRepository users
    )
    {
        _positions = positions;
        _cvs = cvs;
        _tags = tags;
        _users = users;
    }

    public async Task<Result<HomeDashboardDto>> Handle(GetHomeDashboardQuery request, CancellationToken cancellationToken)
    {
        var latest = await _positions.ListAsync(new LatestPositionsSpec(), cancellationToken);
        var top = await _positions.ListAsync(new TopPositionsSpec(), cancellationToken);
        var tagCloud = await _tags.ListAsync(new TagCloudSpec(), cancellationToken);

        var totalPositions = await _positions.CountAsync(cancellationToken);
        var submittedCvs = await _cvs.CountAsync(cancellationToken);
        var cvsLast24h = await _cvs.CountAsync(
            new CvsCreatedSinceSpec(DateTime.UtcNow.AddHours(-24)), cancellationToken);

        var candidates = await _users.CountUsersInRoleAsync(UserRoles.Candidate, cancellationToken);
        var recruiters = await _users.CountUsersInRoleAsync(UserRoles.Recruiter, cancellationToken);

        return Result<HomeDashboardDto>.Success(new HomeDashboardDto(
            latest,
            top,
            tagCloud,
            new HomeStatsDto(totalPositions, candidates, recruiters, submittedCvs, cvsLast24h)
        ));
    }
}
