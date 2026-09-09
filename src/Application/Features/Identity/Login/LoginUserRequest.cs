using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Identity.Login;

public record LoginUserRequest(
    string Email,
    string Password
) : IRequest<Result>;