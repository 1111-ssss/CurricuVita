using Domain.Interfaces.Identity;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Identity.Login;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result>
{
    private readonly IIdentityService _identityService;

    public LoginUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.Login(request.Email, request.Password, cancellationToken);

        return result;
    }
}