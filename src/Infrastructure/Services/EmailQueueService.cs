using System.Threading.Channels;
using Domain.Contracts;
using Domain.Interfaces.Services;
using Infrastructure.Constants;

namespace Infrastructure.Services;

public class EmailQueueService : IEmailQueueService
{
    private readonly Channel<EmailMessageRequest> _queue;

    public EmailQueueService()
    {
        var options = new BoundedChannelOptions(EmailQueueServiceConstants.EmailQueueCapacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<EmailMessageRequest>(options);
    }

    public async ValueTask EnqueueEmail(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default
    )
    {
        await _queue.Writer.WriteAsync(request, cancellationToken);
    }

    public async ValueTask<EmailMessageRequest> DequeueEmail(
        CancellationToken cancellationToken
    )
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}