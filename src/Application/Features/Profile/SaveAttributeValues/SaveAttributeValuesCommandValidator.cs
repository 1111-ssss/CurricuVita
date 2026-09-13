using FluentValidation;

namespace Application.Features.Profile.SaveAttributeValues;

public class SaveAttributeValuesCommandValidator : AbstractValidator<SaveAttributeValuesCommand>
{
    public SaveAttributeValuesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.Items)
            .NotNull()
            .WithMessage("Values are required.")
            .WithErrorCode("ValidationFailed");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.AttributeDefinitionId)
                .GreaterThan(0)
                .WithMessage("Attribute is required.")
                .WithErrorCode("ValidationFailed");

            item.RuleFor(i => i.StringValue)
                .MaximumLength(2000)
                .WithMessage("Value must be at most 2000 characters.")
                .WithErrorCode("ValidationFailed");

            item.RuleFor(i => i.TextValue)
                .MaximumLength(20000)
                .WithMessage("Value must be at most 20000 characters.")
                .WithErrorCode("ValidationFailed");

            item.RuleFor(i => i.ImageValue)
                .MaximumLength(2000)
                .WithMessage("Image URL must be at most 2000 characters.")
                .WithErrorCode("ValidationFailed");

            item.RuleFor(i => i.DropdownValue)
                .MaximumLength(500)
                .WithMessage("Value must be at most 500 characters.")
                .WithErrorCode("ValidationFailed");
        });
    }
}
