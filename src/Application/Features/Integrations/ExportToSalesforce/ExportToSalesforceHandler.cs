using Domain.Contracts.SalesforceContracts;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.Interfaces.Services;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Integrations.ExportToSalesforce;

public class ExportToSalesforceHandler : IRequestHandler<ExportToSalesforceCommand, Result<SalesforceExportResponse>>
{
    private readonly IUserRepository _users;
    private readonly ISalesforceService _salesforce;

    public ExportToSalesforceHandler(
        IUserRepository users,
        ISalesforceService salesforce
    )
    {
        _users = users;
        _salesforce = salesforce;
    }

    public async Task<Result<SalesforceExportResponse>> Handle(
        ExportToSalesforceCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _users.GetByIdAsync(
            request.RequestedByUserId,
            cancellationToken
        );
        if (user is null)
        {
            return Result<SalesforceExportResponse>.Failure(Errors.UserNotFound);
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            return Result<SalesforceExportResponse>.Failure(Errors.EmailRequired);
        }

        var exportRequest = new SalesforceExportRequest(
            user.FirstName,
            user.LastName,
            user.Email,
            user.Location,
            request.Company,
            request.Phone,
            request.Notes
        );

        return await _salesforce.ExportUser(
            exportRequest,
            cancellationToken
        );
    }
}
