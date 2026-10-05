using Domain.Contracts.SalesforceContracts;
using Domain.ResultPattern.Result;

namespace Domain.Interfaces.Services;

public interface ISalesforceService
{
    Task<Result<SalesforceExportResponse>> ExportUser(
        SalesforceExportRequest request,
        CancellationToken cancellationToken = default
    );
}
