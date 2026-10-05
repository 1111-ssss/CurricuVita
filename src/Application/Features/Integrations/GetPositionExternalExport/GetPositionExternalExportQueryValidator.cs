using FluentValidation;

namespace Application.Features.Integrations.GetPositionExternalExport;

public class GetPositionExternalExportQueryValidator : AbstractValidator<GetPositionExternalExportQuery>
{
    public GetPositionExternalExportQueryValidator()
    {
        RuleFor(x => x.ApiToken)
            .NotEmpty()
            .WithMessage("API token is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.ApiToken)
            .MaximumLength(1024)
            .WithMessage("API token must be at most 1024 characters.")
            .WithErrorCode("ValidationFailed");
    }
}
