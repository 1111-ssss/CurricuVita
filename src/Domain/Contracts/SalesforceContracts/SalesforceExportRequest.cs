namespace Domain.Contracts.SalesforceContracts;

public record SalesforceExportRequest(
    string FirstName,
    string LastName,
    string Email,
    string? Location,
    string Company,
    string? Phone,
    string? Notes
);
