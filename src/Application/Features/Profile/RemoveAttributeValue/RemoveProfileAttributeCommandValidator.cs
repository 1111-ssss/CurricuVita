using FluentValidation;

namespace Application.Features.Profile.RemoveAttributeValue;

public class RemoveProfileAttributeCommandValidator : AbstractValidator<RemoveProfileAttributeCommand>
{
    public RemoveProfileAttributeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.ValueId)
            .GreaterThan(0)
            .WithMessage("Value is required.")
            .WithErrorCode("ValidationFailed");
    }
}