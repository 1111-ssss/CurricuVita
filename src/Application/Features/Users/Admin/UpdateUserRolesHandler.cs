using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public class UpdateUserRolesHandler : IRequestHandler<UpdateUserRolesCommand, Result>
{
    private readonly IIdentityService _identity;

    public UpdateUserRolesHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task<Result> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsAdmin)
        {
            return Result.Failure(Errors.UserAdminForbidden);
        }

        return await _identity.UpdateUserRolesAsync(request.TargetUserId, request.Roles, request.RequesterUserId);
    }
}
