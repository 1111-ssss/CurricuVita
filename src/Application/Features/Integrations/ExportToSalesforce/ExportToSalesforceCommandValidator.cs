using FluentValidation;

namespace Application.Features.Integrations.ExportToSalesforce;

public class ExportToSalesforceCommandValidator : AbstractValidator<ExportToSalesforceCommand>
{
    public ExportToSalesforceCommandValidator()
    {
        RuleFor(x => x.RequestedByUserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Company)
            .NotEmpty()
            .WithMessage("Company is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Company)
            .MaximumLength(200)
            .WithMessage("Company must be at most 200 characters.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Phone)
            .MaximumLength(50)
            .WithMessage("Phone must be at most 50 characters.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .WithMessage("Notes must be at most 2000 characters.")
            .WithErrorCode("ValidationFailed");
    }
}
