using System.Text;
using System.Text.Json;
using Domain.Constants;
using Domain.Contracts.SupportContracts;
using Domain.Interfaces.Services;
using Domain.Options;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class PowerAutomateRelaySink : ISupportTicketSink
{
    private readonly SupportTicketOptions _options;
    private readonly HttpClient _http;

    public PowerAutomateRelaySink(
        IOptions<SupportTicketOptions> options,
        HttpClient http
    )
    {
        _options = options.Value;
        _http = http;
    }

    public async Task<Result> Store(
        string fileName,
        SupportTicketPayload payload,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(_options.PowerAutomateWebhookUrl))
        {
            return Result.Success();
        }

        try
        {
            using var content = new StringContent(
                JsonSerializer.Serialize(new { fileName, ticket = payload }),
                Encoding.UTF8,
                SupportTicketConstants.JsonMediaType
            );
            var response = await _http.PostAsync(
                _options.PowerAutomateWebhookUrl,
                content,
                cancellationToken
            );
            return response.IsSuccessStatusCode
                ? Result.Success()
                : Result.Failure(Errors.SupportTicketRelayError);
        }
        catch
        {
            return Result.Failure(Errors.SupportTicketRelayError);
        }
    }
}
