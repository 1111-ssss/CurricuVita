using Domain.Interfaces.Services;
using Infrastructure.Interfaces;

namespace Web.BackgroundServices;

public class EmailBackgroundService : BackgroundService
{
    private readonly IEmailQueueService _emailQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailBackgroundService> _logger;

    public EmailBackgroundService(
        IEmailQueueService emailQueue,
        IServiceScopeFactory scopeFactory,
        ILogger<EmailBackgroundService> logger)
    {
        _emailQueue = emailQueue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Background Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var request = await _emailQueue.DequeueEmail(stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

                await emailSender.SendEmail(
                    request,
                    stoppingToken
                );
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing email from queue");
            }
        }

        _logger.LogInformation("Email Background Worker stopped.");
    }
}