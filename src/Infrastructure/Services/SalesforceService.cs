using System.Text;
using Domain.Constants;
using Domain.Contracts.SalesforceContracts;
using Domain.Interfaces.Services;
using Domain.Options;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class SalesforceService : ISalesforceService
{
    private readonly SalesforceOptions _options;
    private readonly SalesforceAuthClient _auth;
    private readonly SalesforceSObjectClient _objects;

    public SalesforceService(
        IOptions<SalesforceOptions> options,
        SalesforceAuthClient auth,
        SalesforceSObjectClient objects
    )
    {
        _options = options.Value;
        _auth = auth;
        _objects = objects;
    }

    public async Task<Result<SalesforceExportResponse>> ExportUser(
        SalesforceExportRequest request,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(_options.InstanceUrl)
            || string.IsNullOrWhiteSpace(_options.ClientId)
            || string.IsNullOrWhiteSpace(_options.ClientSecret))
        {
            return Result<SalesforceExportResponse>.Failure(Errors.SalesforceNotConfigured);
        }

        var session = await _auth.Acquire(
            cancellationToken
        );
        if (!session.IsSuccess)
        {
            return Result<SalesforceExportResponse>.Failure(session.Error!);
        }

        var apiBase = session.Value.InstanceUrl.TrimEnd('/') + SalesforceConstants.DataPath + _options.ApiVersion;

        var accountId = await _objects.Create(
            apiBase,
            session.Value.AccessToken,
            SalesforceConstants.AccountObject,
            BuildAccountPayload(request),
            cancellationToken
        );
        if (accountId is null)
        {
            return Result<SalesforceExportResponse>.Failure(Errors.SalesforceApiError);
        }

        var contactId = await _objects.Create(
            apiBase,
            session.Value.AccessToken,
            SalesforceConstants.ContactObject,
            BuildContactPayload(
                request,
                accountId
            ),
            cancellationToken
        );
        if (contactId is null)
        {
            return Result<SalesforceExportResponse>.Failure(Errors.SalesforceApiError);
        }

        return Result<SalesforceExportResponse>.Success(new SalesforceExportResponse(accountId, contactId));
    }

    private static Dictionary<string, object?> BuildAccountPayload(
        SalesforceExportRequest request
    )
    {
        return new Dictionary<string, object?>
        {
            [SalesforceConstants.FieldName] = BuildAccountName(request),
            [SalesforceConstants.FieldDescription] = BuildDescription(request),
            [SalesforceConstants.FieldPhone] = request.Phone,
        };
    }

    private static Dictionary<string, object?> BuildContactPayload(
        SalesforceExportRequest request,
        string accountId
    )
    {
        return new Dictionary<string, object?>
        {
            [SalesforceConstants.FieldFirstName] = request.FirstName,
            [SalesforceConstants.FieldLastName] = string.IsNullOrWhiteSpace(request.LastName)
                ? SalesforceConstants.UnknownLastName
                : request.LastName,
            [SalesforceConstants.FieldEmail] = request.Email,
            [SalesforceConstants.FieldPhone] = request.Phone,
            [SalesforceConstants.FieldMailingCity] = request.Location,
            [SalesforceConstants.FieldDescription] = request.Notes,
            [SalesforceConstants.FieldAccountId] = accountId,
        };
    }

    private static string BuildAccountName(
        SalesforceExportRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Company))
        {
            return $"{request.FirstName} {request.LastName} ({request.Email})";
        }
        return request.Company;
    }

    private static string BuildDescription(
        SalesforceExportRequest request
    )
    {
        var builder = new StringBuilder();
        builder.Append($"{SalesforceConstants.SourceName} import. User: {request.FirstName} {request.LastName} <{request.Email}>.");
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            builder.Append($"{SalesforceConstants.LocationPrefix}{request.Location}.");
        }
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            builder.Append($"{SalesforceConstants.NotesPrefix}{request.Notes}");
        }
        return builder.ToString();
    }
}
