using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
public class RegisterValidator : AbstractValidator<RegisterDTO>
{
    public RegisterValidator()
    {
        RuleFor(task => task.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_USERNAME_EMPTY)
            .MaximumLength(100)
            .WithMessage(ResourceErrorMessages.ERROR_USERNAME_TOO_LONG)
            .WithName(nameof(RegisterDTO.Username));

        RuleFor(task => task.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.ERROR_INVALID_EMAIL)
            .MaximumLength(100)
            .WithMessage(ResourceErrorMessages.ERROR_EMAIL_TOO_LONG)
            .WithName(nameof(RegisterDTO.Email));

        RuleFor(task => task.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_PASSWORD_EMPTY)
            .MaximumLength(100)
            .WithMessage(ResourceErrorMessages.ERROR_PASSWORD_TOO_LONG)
            .WithName(nameof(RegisterDTO.Password));
    }
}