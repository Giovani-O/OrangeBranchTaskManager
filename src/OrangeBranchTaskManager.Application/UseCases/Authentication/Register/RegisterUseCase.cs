using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
public class RegisterUseCase
{
    private readonly ITokenServiceUseCase _tokenService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserModel> _userManager;

    public RegisterUseCase(
        ITokenServiceUseCase tokenService,
        IConfiguration configuration,
        UserManager<UserModel> userManager
        //RoleManager<IdentityRole> roleManager
    )
    {
        _tokenService = tokenService;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<IdentityResult> Execute(RegisterDTO registerData)
    {
        Validate(registerData);

        var existentUser = await _userManager.FindByEmailAsync(registerData.Email!);
        if (existentUser is not null) 
            throw new ErrorOnExecutionException([ResourceErrorMessages.ERROR_EMAIL_ALREADY_EXISTS]);

        UserModel user = new()
        {
            UserName = registerData.Username,
            Email = registerData.Email,
            NormalizedEmail = _userManager.NormalizeEmail(registerData.Email)
        };

        var result = await _userManager.CreateAsync(user, registerData.Password!);

        if (!result.Succeeded) throw new ErrorOnExecutionException([ResourceErrorMessages.ERROR_CREATE_USER]);

        return result;
    }

    private void Validate(RegisterDTO registerData) 
    {
        var validator = new RegisterValidator();
        var result = validator.Validate(registerData);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
