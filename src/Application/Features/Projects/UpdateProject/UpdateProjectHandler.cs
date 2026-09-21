using AutoMapper;
using Domain.Contracts.UserContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Projects.UpdateProject;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
{
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;

    public UpdateProjectHandler(
        IProjectRepository projects,
        IMapper mapper
    )
    {
        _projects = projects;
        _mapper = mapper;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var result = await _projects.TryUpdateProjectAsync(
            request.ProjectId,
            request.UserId,
            request.Title.Trim(),
            request.StartDate,
            request.EndDate,
            request.DescriptionMarkdown ?? string.Empty,
            request.Tags,
            request.Version,
            cancellationToken
        );

        return !result.IsSuccess
            ? Result<ProjectDto>.Failure(result.Error!)
            : Result<ProjectDto>.Success(_mapper.Map<ProjectDto>(result.Value));
    }
}
