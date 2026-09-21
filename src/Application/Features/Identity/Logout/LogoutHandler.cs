using Domain.Interfaces.Identity;
using MediatR;

namespace Application.Features.Identity.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly IIdentityService _identityService;

    public LogoutHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _identityService.Logout();
    }
}