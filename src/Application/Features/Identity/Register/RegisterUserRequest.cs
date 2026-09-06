using MediatR;
using Domain.ResultPattern.Result;

namespace Application.Features.Identity.Register;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Location,
    string Email,
    string Password
) : IRequest<Result>;