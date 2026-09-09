using Domain.Contracts;

namespace Infrastructure.Interfaces;

public interface IEmailSenderService
{
    Task SendEmail(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default
    );
}