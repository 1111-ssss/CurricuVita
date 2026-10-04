namespace Domain.Contracts.SalesforceContracts;

public record SalesforceExportResponse(
    string AccountId,
    string ContactId
);
