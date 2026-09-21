using Domain.Contracts.UserContracts;
using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public class ListUsersHandler : IRequestHandler<ListUsersQuery, Result<List<UserAdminDto>>>
{
    private readonly IIdentityService _identity;

    public ListUsersHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task<Result<List<UserAdminDto>>> Handle(
        ListUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!request.IsAdmin)
        {
            return Result<List<UserAdminDto>>.Failure(Errors.UserAdminForbidden);
        }

        return await _identity.ListUsersAsync(request.Search, request.Take, request.Skip, cancellationToken);
    }
}
