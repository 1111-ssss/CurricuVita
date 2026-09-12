using Domain.Interfaces.Identity;
using Domain.ResultPattern.Result;
using Domain.ResultPattern.Errors;
using Domain.Interfaces.Services;
using Domain.Options;
using MediatR;
using Domain.Contracts;
using Application.Constants;
using Microsoft.Extensions.Options;

namespace Application.Features.Identity.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUser;
    private readonly IEmailQueueService _emailQueue;
    private readonly IEmailTemplateRenderer _emailTemplateRenderer;
    private readonly AppOptions _appOptions;

    public RegisterUserHandler(
        IIdentityService identityService,
        ICurrentUserService currentUser,
        IEmailQueueService emailQueue,
        IEmailTemplateRenderer emailTemplateRenderer,
        IOptions<AppOptions> appOptions
    )
    {
        _identityService = identityService;
        _currentUser = currentUser;
        _emailQueue = emailQueue;
        _emailTemplateRenderer = emailTemplateRenderer;
        _appOptions = appOptions.Value;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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
            return Result.Failure(emailConfirmationToken.Error!);
        }

        var baseUrl = string.IsNullOrWhiteSpace(_appOptions.BaseUrl)
            ? _currentUser.GetBaseUrl()
            : _appOptions.BaseUrl.TrimEnd('/');
            
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return Result.Failure(Errors.EmailConfirmationLinkFailed);
        }

        var confirmUrl = $"{baseUrl}{EmailTemplateConstants.ConfirmEmailUrl}?id={result.Value}&token={Uri.EscapeDataString(emailConfirmationToken.Value)}";

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