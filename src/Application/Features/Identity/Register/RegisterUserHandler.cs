using Domain.Interfaces.Identity;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Identity.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, Result>
{
    private readonly IIdentityService _identityService;

    public RegisterUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterUser(
            email: request.Email,
            firstName: request.FirstName,
            lastName: request.LastName,
            password: request.Password,
            cancellationToken: cancellationToken
        );

        return result;
    }
}