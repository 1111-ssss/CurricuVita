using FluentValidation;

namespace Application.Features.Likes.ToggleLike;

public class ToggleLikeCvCommandValidator : AbstractValidator<ToggleLikeCvCommand>
{
    public ToggleLikeCvCommandValidator()
    {
        RuleFor(x => x.CvId)
            .GreaterThan(0)
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.RequesterUserId)
            .GreaterThan(0)
            .WithErrorCode("ValidationFailed");
    }
}
