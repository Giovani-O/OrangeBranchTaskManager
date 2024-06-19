using FluentValidation;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Delete;

public class DeleteTaskValidator : AbstractValidator<int>
{
    public DeleteTaskValidator()
    {
        RuleFor(task => task).GreaterThan(0).WithMessage(ResourceErrorMessages.ERROR_INVALID_ID);
    }
}
