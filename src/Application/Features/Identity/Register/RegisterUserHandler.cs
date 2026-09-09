using Domain.Interfaces.Identity;
using Domain.ResultPattern.Result;
using Domain.Interfaces.Services;
using MediatR;
using Domain.Contracts;
using Application.Constants;

namespace Application.Features.Identity.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, Result>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUser;
    private readonly IEmailQueueService _emailQueue;
    private readonly IEmailTemplateRenderer _emailTemplateRenderer;

    public RegisterUserHandler(
        IIdentityService identityService,
        ICurrentUserService currentUser,
        IEmailQueueService emailQueue,
        IEmailTemplateRenderer emailTemplateRenderer
    )
    {
        _identityService = identityService;
        _currentUser = currentUser;
        _emailQueue = emailQueue;
        _emailTemplateRenderer = emailTemplateRenderer;
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

        if (!result.IsSuccess)
        {
            return result;
        }

        var emailConfirmationToken = await _identityService.GenerateEmailConfirmationToken(result.Value);
        if (!emailConfirmationToken.IsSuccess)
        {
            return result;
        }

        var baseUrl = _currentUser.GetBaseUrl();
        var confirmUrl = $"{baseUrl}{EmailTemplateConstants.ConfirmEmailUrl}?id={result.Value}?token={emailConfirmationToken.Value}";

        var emailBody = await _emailTemplateRenderer.Render(
            EmailTemplateConstants.ConfirmEmail,
            new ConfirmEmailTemplateModel
            {
                FirstName = request.FirstName,
                ConfirmEmail = confirmUrl
            }
        );

        await _emailQueue.EnqueueEmail(new EmailMessageRequest(
            ToEmail: request.Email,
            Subject: EmailTemplateConstants.ConfirmEmailSubject,
            Body: emailBody
        ));

        return result;
    }
}