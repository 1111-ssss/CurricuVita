using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Users.Admin;

public record ListUsersQuery(
    int RequesterUserId,
    bool IsAdmin,
    string? Search = null,
    int Take = 100,
    int Skip = 0
) : IRequest<Result<List<UserAdminDto>>>;
