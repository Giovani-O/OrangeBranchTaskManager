using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;

public class GetTaskByIdValidator : AbstractValidator<int>
{
    public GetTaskByIdValidator()
    {
        RuleFor(task => task)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.ERROR_INVALID_ID)
            .WithName(nameof(TaskDTO.Id));
    }
}