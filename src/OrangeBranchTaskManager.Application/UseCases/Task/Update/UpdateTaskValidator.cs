using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Update;

public class UpdateTaskValidator : AbstractValidator<TaskDTO>
{
    public UpdateTaskValidator()
    {

        RuleFor(task => task.Id)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.ERROR_INVALID_ID)
            .WithName(nameof(TaskDTO.Id));

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
