using Domain.Enums;
using Domain.Helpers;
using FluentValidation;

namespace Application.Features.Attributes.CreateAttribute;

public class CreateAttributeCommandValidator : AbstractValidator<CreateAttributeCommand>
{
    public CreateAttributeCommandValidator()
    {
        RuleFor(x => x.Category)
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage("Category is required.")
            .WithErrorCode("AttributeInvalidCategory");
        RuleFor(x => x.Category)
            .MaximumLength(100)
            .WithMessage("Category must be at most 100 characters.")
            .WithErrorCode("AttributeInvalidCategory");

        RuleFor(x => x.Name)
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name is required.")
            .WithErrorCode("AttributeInvalidName");
        RuleFor(x => x.Name)
            .MaximumLength(150)
            .WithMessage("Name must be at most 150 characters.")
            .WithErrorCode("AttributeInvalidName");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must be at most 1000 characters.")
            .WithErrorCode("AttributeInvalidDescription");

        RuleFor(x => x)
            .Must(HaveValidOptions)
            .WithMessage("Dropdown attributes require at least one option; other types must not have options.")
            .WithErrorCode("AttributeInvalidOptions");
    }

    private static bool HaveValidOptions(CreateAttributeCommand command)
    {
        var hasValues = command.Options?.Any(o => !string.IsNullOrWhiteSpace(o)) == true;
        return command.DataType == AttributeDataType.Dropdown
            ? AttributeOptionsHelper.NormalizeOptions(command.DataType, command.Options).Count > 0
            : !hasValues;
    }
}
