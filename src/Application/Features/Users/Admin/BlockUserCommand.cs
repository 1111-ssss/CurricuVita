using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public record BlockUserCommand(
    int TargetUserId,
    int RequesterUserId,
    bool IsAdmin
) : IRequest<Result>;
