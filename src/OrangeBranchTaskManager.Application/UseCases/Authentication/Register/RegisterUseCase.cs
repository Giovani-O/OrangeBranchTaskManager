using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Application.UseCases.SendMessage;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Communication.Templates;
using System.Text;
using System.Text.Json;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
public class RegisterUseCase
{
    private readonly ITokenServiceUseCase _tokenService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserModel> _userManager;
    private readonly IRabbitMQConnectionManager _connectionManager;

    public RegisterUseCase(
        ITokenServiceUseCase tokenService,
        IConfiguration configuration,
        UserManager<UserModel> userManager,
        IRabbitMQConnectionManager connectionManager
        //RoleManager<IdentityRole> roleManager
    )
    {
        _tokenService = tokenService;
        _configuration = configuration;
        _userManager = userManager;
        _connectionManager = connectionManager;
    }

    public async Task<IdentityResult> Execute(RegisterDTO registerData)
    {
        Validate(registerData);

        var existentUser = await _userManager.FindByEmailAsync(registerData.Email!);
        if (existentUser is not null) 
            throw new ErrorOnExecutionException(
                new Dictionary<string, List<string>>()
                {
                    { nameof(RegisterDTO.Email), new List<string> { ResourceErrorMessages.ERROR_EMAIL_ALREADY_EXISTS } }
                }
            );

        UserModel user = new()
        {
            UserName = registerData.Username,
            Email = registerData.Email,
            NormalizedEmail = _userManager.NormalizeEmail(registerData.Email)
        };

        var result = await _userManager.CreateAsync(user, registerData.Password!);

        if (!result.Succeeded) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>()
            {
                { "Error", new List<string> { ResourceErrorMessages.ERROR_CREATE_USER } }
            }
        );

        await SendMessage(registerData.Username, registerData.Email);

        return result;
    }

    private void Validate(RegisterDTO registerData) 
    {
        var validator = new RegisterValidator();
        var result = validator.Validate(registerData);

        if (!result.IsValid)
        {
            //var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            //throw new ErrorOnValidationException(errorMessages);
            var errorDictionary = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

            throw new ErrorOnValidationException(errorDictionary);
        }
    }

    private async Task SendMessage(string username, string email)
    {
        var messageInfo = new EmailTemplate 
        { 
            NotificationType = Domain.Enums.NotificationType.NewUser,
            Username = username,
            Email = email,
        };

        var messageJson = JsonSerializer.Serialize(messageInfo);

        var message = new SendMessageDTO
        {
            Message = messageJson
        };

        var sendMessageUseCase = new SendMessageUseCase(_connectionManager);

        await sendMessageUseCase.Execute(message);
    }
}
