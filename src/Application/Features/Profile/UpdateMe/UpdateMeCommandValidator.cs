using FluentValidation;

namespace Application.Features.Profile.UpdateMe;

public class UpdateMeCommandValidator : AbstractValidator<UpdateMeCommand>
{
    public UpdateMeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.FirstName)
            .MaximumLength(100)
            .WithMessage("First name must be at most 100 characters.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .WithMessage("Last name must be at most 100 characters.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.Location)
            .MaximumLength(200)
            .WithMessage("Location must be at most 200 characters.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(2000)
            .WithMessage("Avatar URL must be at most 2000 characters.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Version is required.")
            .WithErrorCode("ValidationFailed");
    }
}
