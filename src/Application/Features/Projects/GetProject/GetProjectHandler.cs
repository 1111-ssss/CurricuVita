using AutoMapper;
using Domain.Contracts.UserContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Projects.GetProject;

public class GetProjectHandler : IRequestHandler<GetProjectQuery, Result<ProjectDto>>
{
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;

    public GetProjectHandler(
        IProjectRepository projects, 
        IMapper mapper
    )
    {
        _projects = projects;
        _mapper = mapper;
    }

    public async Task<Result<ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetProjectAsync(request.ProjectId, request.UserId, cancellationToken);

        return project is null
            ? Result<ProjectDto>.Failure(Errors.NotFound)
            : Result<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
    }
}
