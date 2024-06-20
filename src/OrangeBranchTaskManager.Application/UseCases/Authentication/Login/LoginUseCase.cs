using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
public class LoginUseCase
{
    private readonly ITokenServiceUseCase _tokenService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserModel> _userManager;

    public LoginUseCase(
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

    public async Task<LoginResponseDTO> Execute(LoginDTO loginData)
    {
        Validate(loginData);

        var user = await _userManager.FindByEmailAsync(loginData.Email!);

        if (user is null) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>() 
            {
                { nameof(LoginDTO.Email), new List<string> { ResourceErrorMessages.ERROR_NOT_FOUND_USER } }
            }
        );

        if (await _userManager.CheckPasswordAsync(user, loginData.Password) == false)
            throw new ErrorOnExecutionException(
                new Dictionary<string, List<string>>() 
                {
                    { nameof(LoginDTO.Password), new List<string> { ResourceErrorMessages.ERROR_INVALID_PASSWORD } }
                }
            );

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = GenerateClaimsList(user);

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = _tokenService.Execute(authClaims, _configuration);

        await _userManager.UpdateAsync(user);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        var tokenExpiration = new JwtSecurityToken(jwtToken).ValidTo;

        return new LoginResponseDTO
        {
            Token = jwtToken,
            ValidTo = tokenExpiration,
            UserName = user.UserName!
        };
    }

    private void Validate(LoginDTO loginData)
    {
        var validator = new LoginValidator();
        var result = validator.Validate(loginData);

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

    private List<Claim> GenerateClaimsList(UserModel user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("id", user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
    }
}
