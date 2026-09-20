using FluentValidation;

namespace Application.Features.Cvs.GetMyCv;

public class GetMyCvQueryValidator : AbstractValidator<GetMyCvQuery>
{
    public GetMyCvQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage("Position is required.")
            .WithErrorCode("ValidationFailed");
    }
}
