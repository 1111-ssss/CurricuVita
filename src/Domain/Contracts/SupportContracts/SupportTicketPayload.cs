using System.Text.Json.Serialization;

namespace Domain.Contracts.SupportContracts;

public record SupportTicketPayload(
    [property: JsonPropertyName("Reported by")] string ReportedBy,
    [property: JsonPropertyName("Reporter roles")] List<string> ReporterRoles,
    [property: JsonPropertyName("Position")] string? Position,
    [property: JsonPropertyName("Link")] string Link,
    [property: JsonPropertyName("Priority")] string Priority,
    [property: JsonPropertyName("Summary")] string Summary,
    [property: JsonPropertyName("Admin e-mails")] List<string> AdminEmails,
    [property: JsonPropertyName("Created at")] DateTime CreatedAt
);
