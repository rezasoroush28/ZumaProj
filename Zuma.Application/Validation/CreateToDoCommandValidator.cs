using FluentValidation;
using Zuma.Application.Commands;

public class CreateToDoCommandValidator : AbstractValidator<CreateToDoCommandRequest>
{
    public CreateToDoCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.status)
            .IsInEnum().WithMessage("Invalid status value.");
    }
}
