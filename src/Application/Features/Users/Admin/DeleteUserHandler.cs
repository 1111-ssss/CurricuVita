using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IIdentityService _identity;

    public DeleteUserHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsAdmin)
        {
            return Result.Failure(Errors.UserAdminForbidden);
        }

        return await _identity.DeleteUserAsync(request.TargetUserId, request.RequesterUserId);
    }
}
