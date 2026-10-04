namespace Domain.Contracts.SalesforceContracts;

public record SalesforceSession(
    string AccessToken,
    string InstanceUrl
);
