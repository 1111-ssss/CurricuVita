using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Projects.CreateProject;

public record CreateProjectCommand(
    int UserId,
    string Title,
    DateTime? StartDate,
    DateTime? EndDate,
    string DescriptionMarkdown,
    List<string> Tags
) : IRequest<Result<ProjectDto>>;
