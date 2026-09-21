using FluentValidation;

namespace Application.Features.Identity.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
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
            .MaximumLength(256)
            .WithMessage("Password must be at most 256 characters.")
            .WithErrorCode("ValidationFailed");
    }
}
