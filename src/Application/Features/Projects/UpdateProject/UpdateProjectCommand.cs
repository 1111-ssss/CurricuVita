using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Projects.UpdateProject;

public record UpdateProjectCommand(
    int ProjectId,
    int UserId,
    string Title,
    DateTime? StartDate,
    DateTime? EndDate,
    string DescriptionMarkdown,
    List<string> Tags,
    int Version
) : IRequest<Result<ProjectDto>>;
