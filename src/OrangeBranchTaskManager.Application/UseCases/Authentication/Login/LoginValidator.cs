﻿using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
public class LoginValidator : AbstractValidator<LoginDTO>
{
    public LoginValidator()
    {
        RuleFor(task => task.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.ERROR_INVALID_EMAIL)
            .WithName(nameof(LoginDTO.Email));

        RuleFor(task => task.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_PASSWORD_EMPTY)
            .WithName(nameof(LoginDTO.Password));
    }
}
