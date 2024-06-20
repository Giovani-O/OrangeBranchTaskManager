using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Create;

public class CreateTaskValidator : AbstractValidator<TaskDTO>
{
    public CreateTaskValidator()
    {
        RuleFor(task => task.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_TITLE_EMPTY)
            .MaximumLength(100)
            .WithMessage(ResourceErrorMessages.ERROR_TITLE_TOO_LONG)
            .WithName(nameof(TaskDTO.Title));

        RuleFor(task => task.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_DESCRIPTION_EMPTY)
            .MaximumLength(300)
            .WithMessage(ResourceErrorMessages.ERROR_DESCRIPTION_TOO_LONG)
            .WithName(nameof(TaskDTO.Description));

        RuleFor(task => task.DueDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_DUE_DATE_EMPTY)
            .GreaterThan(DateTime.Now)
            .WithMessage(ResourceErrorMessages.ERROR_DUE_DATE_PAST)
            .WithName(nameof(TaskDTO.DueDate));
    }
}
