using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using FluentValidation;
using MediatR;

namespace Application.Features.Projects.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, Result>
{
    private readonly IProjectRepository _projects;

    public DeleteProjectHandler(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var result = await _projects.TryDeleteProjectAsync(request.ProjectId, request.UserId, request.Version, cancellationToken);

        return result;
    }
}
