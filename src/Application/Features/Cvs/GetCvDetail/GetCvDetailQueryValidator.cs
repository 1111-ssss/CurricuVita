using FluentValidation;

namespace Application.Features.Cvs.GetCvDetail;

public class GetCvDetailQueryValidator : AbstractValidator<GetCvDetailQuery>
{
    public GetCvDetailQueryValidator()
    {
        RuleFor(x => x.CvId)
            .GreaterThan(0)
            .WithMessage("CV is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.RequesterUserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");
    }
}
