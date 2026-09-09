using Domain.Contracts;

namespace Domain.Interfaces.Services;

public interface IEmailQueueService
{
    ValueTask EnqueueEmail(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default
    );
    ValueTask<EmailMessageRequest> DequeueEmail(
        CancellationToken cancellationToken = default
    );
}