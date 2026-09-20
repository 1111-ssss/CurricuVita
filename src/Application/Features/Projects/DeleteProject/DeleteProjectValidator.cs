using FluentValidation;

namespace Application.Features.Projects.DeleteProject;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("Project is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");

        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Version is required.")
            .WithErrorCode("ValidationFailed");
    }
}