using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.SendEmail;

public class SendEmailValidator : AbstractValidator<TaskDTO>
{
    public SendEmailValidator()
    {
        RuleFor(task => task.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_TITLE_EMPTY)
            .WithName(nameof(TaskDTO.Title));

        RuleFor(task => task.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_DESCRIPTION_EMPTY)
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