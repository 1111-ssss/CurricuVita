using FluentValidation;

namespace Application.Features.Identity.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
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
            .NotEmpty()
            .WithMessage("Location is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Location)
            .MaximumLength(200)
            .WithMessage("Location must be at most 200 characters.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Email)
            .MaximumLength(100)
            .WithMessage("Email must be at most 100 characters.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is not valid.")
            .WithErrorCode("ValidationFailed")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Password)
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Password)
            .MaximumLength(256)
            .WithMessage("Password must be at most 256 characters.")
            .WithErrorCode("ValidationFailed");
    }
}
