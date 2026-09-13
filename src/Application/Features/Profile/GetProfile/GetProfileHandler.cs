using Ardalis.Specification;
using AutoMapper;
using Domain.Contracts.AttributeContracts;
using Domain.Contracts.UserContracts;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Profile.GetProfile;

public class GetProfileHandler : IRequestHandler<GetProfileQuery, Result<ProfileDto>>
{
    private readonly IUserRepository _users;
    private readonly IRepositoryBase<UserAttributeValue> _values;
    private readonly IProjectRepository _projects;
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IMapper _mapper;

    public GetProfileHandler(
        IUserRepository users,
        IRepositoryBase<UserAttributeValue> values,
        IProjectRepository projects,
        IRepositoryBase<CV> cvs,
        IMapper mapper
    )
    {
        _users = users;
        _values = values;
        _projects = projects;
        _cvs = cvs;
        _mapper = mapper;
    }

    public async Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result<ProfileDto>.Failure(Errors.UserNotFound);
        }

        var values = await _values.ListAsync(new UserAttributeValuesByUserSpec(request.UserId), cancellationToken);
        var projects = await _projects.GetProjectsAsync(request.UserId, cancellationToken);
        var cvs = await _cvs.ListAsync(new CvsByUserSpec(request.UserId), cancellationToken);

        return Result<ProfileDto>.Success(new ProfileDto(
            _mapper.Map<MeDto>(user),
            _mapper.Map<List<ProfileAttributeValueDto>>(values),
            _mapper.Map<List<ProjectDto>>(projects),
            _mapper.Map<List<CvListItemDto>>(cvs)
        ));
    }
}
