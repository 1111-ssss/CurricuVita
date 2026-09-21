using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public record UpdateUserRolesCommand(
    int TargetUserId,
    List<string> Roles,
    int RequesterUserId,
    bool IsAdmin
) : IRequest<Result>;
