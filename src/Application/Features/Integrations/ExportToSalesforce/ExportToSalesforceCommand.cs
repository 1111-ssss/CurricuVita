using Domain.Contracts.SalesforceContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Integrations.ExportToSalesforce;

public record ExportToSalesforceCommand(
    int RequestedByUserId,
    bool RequestedByAdmin,
    string Company,
    string? Phone,
    string? Notes
) : IRequest<Result<SalesforceExportResponse>>;
