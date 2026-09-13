using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Projects.GetProject;

public record GetProjectQuery(
    int ProjectId,
    int UserId
) : IRequest<Result<ProjectDto>>;
