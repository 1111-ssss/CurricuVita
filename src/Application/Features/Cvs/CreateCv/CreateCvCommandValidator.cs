using FluentValidation;

namespace Application.Features.Cvs.CreateCv;

public class CreateCvCommandValidator : AbstractValidator<CreateCvCommand>
{
    public CreateCvCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage("PositionId must be greater than 0.")
            .WithErrorCode("ValidationFailed");
    }
}
