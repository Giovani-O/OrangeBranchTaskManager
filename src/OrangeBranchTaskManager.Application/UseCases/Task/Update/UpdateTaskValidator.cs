using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Update;

public class UpdateTaskValidator : AbstractValidator<TaskDTO>
{
    public UpdateTaskValidator()
    {
        RuleFor(task => task.Id).GreaterThan(0).WithMessage(ResourceErrorMessages.ERROR_INVALID_ID);
        RuleFor(task => task.Title).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_TITLE_EMPTY);
        RuleFor(task => task.Title).MaximumLength(100).WithMessage(ResourceErrorMessages.ERROR_TITLE_TOO_LONG);
        RuleFor(task => task.Description).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_DESCRIPTION_EMPTY);
        RuleFor(task => task.Description).MaximumLength(300).WithMessage(ResourceErrorMessages.ERROR_DESCRIPTION_TOO_LONG);
        RuleFor(task => task.DueDate).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_DUE_DATE_EMPTY);
        RuleFor(task => task.DueDate).GreaterThan(DateTime.Now).WithMessage(ResourceErrorMessages.ERROR_DUE_DATE_PAST);
    }
}
