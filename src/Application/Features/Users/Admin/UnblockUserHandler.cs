using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public class UnblockUserHandler : IRequestHandler<UnblockUserCommand, Result>
{
    private readonly IIdentityService _identity;

    public UnblockUserHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task<Result> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsAdmin)
        {
            return Result.Failure(Errors.UserAdminForbidden);
        }

        return await _identity.UnblockUserAsync(request.TargetUserId, request.RequesterUserId);
    }
}
