using System.Text.Json;
using Domain.Constants;
using Domain.Contracts.SalesforceContracts;
using Domain.Options;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class SalesforceAuthClient
{
    private readonly SalesforceOptions _options;
    private readonly HttpClient _http;
    private readonly ILogger<SalesforceAuthClient> _logger;

    private SalesforceSession? _cached;

    public SalesforceAuthClient(
        IOptions<SalesforceOptions> options,
        HttpClient http,
        ILogger<SalesforceAuthClient> logger
    )
    {
        _options = options.Value;
        _http = http;
        _logger = logger;
    }

    public async Task<Result<SalesforceSession>> Acquire(
        CancellationToken cancellationToken
    )
    {
        if (_cached is not null)
        {
            return Result<SalesforceSession>.Success(_cached);
        }

        var clientCredentials = await AcquireByClientCredentials(
            cancellationToken
        );
        if (clientCredentials.IsSuccess)
        {
            return clientCredentials;
        }

        if (string.IsNullOrWhiteSpace(_options.Username)
            || string.IsNullOrWhiteSpace(_options.Password))
        {
            return clientCredentials;
        }

        return await AcquireByPassword(
            cancellationToken
        );
    }

    private async Task<Result<SalesforceSession>> AcquireByClientCredentials(
        CancellationToken cancellationToken
    )
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            [SalesforceConstants.GrantTypeKey] = SalesforceConstants.ClientCredentialsGrant,
            [SalesforceConstants.ClientIdKey] = _options.ClientId,
            [SalesforceConstants.ClientSecretKey] = _options.ClientSecret,
        });

        var body = await PostToken(
            form,
            cancellationToken
        );
        if (body is null)
        {
            return Result<SalesforceSession>.Failure(Errors.SalesforceApiError);
        }

        return Parse(body);
    }

    private async Task<Result<SalesforceSession>> AcquireByPassword(
        CancellationToken cancellationToken
    )
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            [SalesforceConstants.GrantTypeKey] = SalesforceConstants.PasswordGrant,
            [SalesforceConstants.ClientIdKey] = _options.ClientId,
            [SalesforceConstants.ClientSecretKey] = _options.ClientSecret,
            [SalesforceConstants.UsernameKey] = _options.Username,
            [SalesforceConstants.PasswordKey] = _options.Password + _options.SecurityToken,
        });

        var body = await PostToken(
            form,
            cancellationToken
        );
        if (body is null)
        {
            return Result<SalesforceSession>.Failure(Errors.SalesforceApiError);
        }

        return Parse(body);
    }

    private async Task<string?> PostToken(
        FormUrlEncodedContent form,
        CancellationToken cancellationToken
    )
    {
        var tokenUrl = _options.InstanceUrl.TrimEnd('/') + SalesforceConstants.TokenEndpointPath;

        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsync(
                tokenUrl,
                form,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Salesforce auth request failed");
            return null;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Salesforce auth failed ({Status}): {Body}", (int)response.StatusCode, body);
            return null;
        }

        return body;
    }

    private Result<SalesforceSession> Parse(
        string body
    )
    {
        SalesforceSession session;
        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            var accessToken = root.GetProperty(SalesforceConstants.AccessTokenProperty).GetString();
            var instanceUrl = root.TryGetProperty(SalesforceConstants.InstanceUrlProperty, out var iu)
                ? iu.GetString()
                : _options.InstanceUrl;

            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(instanceUrl))
            {
                return Result<SalesforceSession>.Failure(Errors.SalesforceApiError);
            }

            session = new SalesforceSession(accessToken, instanceUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Salesforce auth parse failed: {Body}", body);
            return Result<SalesforceSession>.Failure(Errors.SalesforceApiError);
        }

        _cached = session;
        return Result<SalesforceSession>.Success(session);
    }
}
