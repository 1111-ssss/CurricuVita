using Domain.Contracts;

namespace Domain.Interfaces.Services;

public interface IEmailSenderService
{
    Task SendEmail(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default
    );
}