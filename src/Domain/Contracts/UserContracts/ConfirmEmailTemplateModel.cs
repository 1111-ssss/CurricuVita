namespace Domain.Contracts.UserContracts;

public class ConfirmEmailTemplateModel
{
    public string FirstName { get; set; } = string.Empty;
    public string ConfirmUrl { get; set; } = string.Empty;
}
