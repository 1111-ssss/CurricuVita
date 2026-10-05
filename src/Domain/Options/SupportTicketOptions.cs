namespace Domain.Options;

public class SupportTicketOptions
{
    public const string SectionName = "SupportTickets";

    public string StorageDir { get; set; } = "support-tickets";
    public string PowerAutomateWebhookUrl { get; set; } = string.Empty;
}
