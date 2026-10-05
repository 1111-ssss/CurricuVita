using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Domain.Constants;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SalesforceSObjectClient
{
    private readonly HttpClient _http;
    private readonly ILogger<SalesforceSObjectClient> _logger;

    public SalesforceSObjectClient(
        HttpClient http,
        ILogger<SalesforceSObjectClient> logger
    )
    {
        _http = http;
        _logger = logger;
    }

    public async Task<string?> Create(
        string apiBase,
        string accessToken,
        string sobject,
        Dictionary<string, object?> payload,
        CancellationToken cancellationToken
    )
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{apiBase}{SalesforceConstants.SObjectsPath}{sobject}"
        );
        request.Headers.Authorization = new AuthenticationHeaderValue(
            SalesforceConstants.BearerScheme,
            accessToken
        );
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            SalesforceConstants.JsonMediaType
        );

        HttpResponseMessage response;
        try
        {
            response = await _http.SendAsync(
                request,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Salesforce create {SObject} failed", sobject);
            return null;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Salesforce create {SObject} failed ({Status}): {Body}", sobject, (int)response.StatusCode, body);
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(body);
            if (document.RootElement.TryGetProperty(SalesforceConstants.IdProperty, out var id))
            {
                return id.GetString();
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Salesforce create {SObject} parse failed: {Body}", sobject, body);
            return null;
        }
    }
}
