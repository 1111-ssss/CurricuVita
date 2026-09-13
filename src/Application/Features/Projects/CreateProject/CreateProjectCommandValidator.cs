using FluentValidation;

namespace Application.Features.Projects.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Title)
            .MaximumLength(200)
            .WithMessage("Title must be at most 200 characters.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.DescriptionMarkdown)
            .MaximumLength(20000)
            .WithMessage("Description must be at most 20000 characters.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.Tags)
            .NotNull()
            .WithMessage("Tags are required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Tags.Count)
            .LessThanOrEqualTo(20)
            .WithMessage("At most 20 tags are allowed.")
            .WithErrorCode("ValidationFailed");
        RuleForEach(x => x.Tags)
            .MaximumLength(100)
            .WithMessage("Tag must be at most 100 characters.")
            .WithErrorCode("ValidationFailed");
    }
}
