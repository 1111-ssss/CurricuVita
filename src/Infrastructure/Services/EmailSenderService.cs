using Domain.Interfaces.Services;
using Domain.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using Domain.Contracts;

namespace Infrastructure.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IOptionsMonitor<EmailSenderOptions> _options;

    public EmailSenderService(IOptionsMonitor<EmailSenderOptions> options)
    {
        _options = options;
    }

    public async Task SendEmail(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var email_from = _options.CurrentValue.From;

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(email_from));
        message.To.Add(MailboxAddress.Parse(request.ToEmail));
        message.Subject = request.Subject;
        message.Body = new TextPart("html")
        {
            Text = request.Body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _options.CurrentValue.SmtpServer,
            _options.CurrentValue.SmtpPort,
            SecureSocketOptions.Auto,
            cancellationToken
        );

        await client.AuthenticateAsync(
            email_from,
            _options.CurrentValue.Password,
            cancellationToken
        );

        await client.SendAsync(message, cancellationToken);

        await client.DisconnectAsync(true, cancellationToken);
    }
}