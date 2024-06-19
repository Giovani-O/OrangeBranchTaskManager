using FluentValidation;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Task.GetById;

public class GetTaskByIdValidator : AbstractValidator<int>
{
    public GetTaskByIdValidator()
    {
        RuleFor(task => task).GreaterThan(0).WithMessage(ResourceErrorMessages.ERROR_INVALID_ID);
    }
}