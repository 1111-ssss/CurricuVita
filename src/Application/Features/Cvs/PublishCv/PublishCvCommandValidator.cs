using FluentValidation;

namespace Application.Features.Cvs.PublishCv;

public class PublishCvCommandValidator : AbstractValidator<PublishCvCommand>
{
    public PublishCvCommandValidator()
    {
        RuleFor(x => x.CvId)
            .GreaterThan(0)
            .WithMessage("CV is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.RequesterUserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.ExpectedVersion)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Version is required.")
            .WithErrorCode("ValidationFailed");
    }
}
