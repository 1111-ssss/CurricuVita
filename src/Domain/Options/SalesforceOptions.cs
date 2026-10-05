namespace Domain.Options;

public class SalesforceOptions
{
    public const string SectionName = "Salesforce";

    public string InstanceUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string SecurityToken { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = "v59.0";
}
