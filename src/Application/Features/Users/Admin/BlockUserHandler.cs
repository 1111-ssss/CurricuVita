using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public class BlockUserHandler : IRequestHandler<BlockUserCommand, Result>
{
    private readonly IIdentityService _identity;

    public BlockUserHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task<Result> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsAdmin)
        {
            return Result.Failure(Errors.UserAdminForbidden);
        }

        return await _identity.BlockUserAsync(request.TargetUserId, request.RequesterUserId);
    }
}
