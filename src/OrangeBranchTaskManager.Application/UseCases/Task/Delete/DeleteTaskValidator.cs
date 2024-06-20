using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Delete;

public class DeleteTaskValidator : AbstractValidator<int>
{
    public DeleteTaskValidator()
    {
        RuleFor(task => task)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.ERROR_INVALID_ID)
            .WithName(nameof(TaskDTO.Id));
    }
}
