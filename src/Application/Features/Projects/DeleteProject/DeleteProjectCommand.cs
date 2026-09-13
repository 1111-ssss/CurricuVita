using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Projects.DeleteProject;

public record DeleteProjectCommand(
    int ProjectId,
    int UserId,
    int Version
) : IRequest<Result>;
