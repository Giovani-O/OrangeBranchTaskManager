using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrangeBranchTaskManager.Exception;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrangeBranchTaskManager.Application.UseCases.Token.TokenService;

public class TokenServiceUseCase : ITokenServiceUseCase
{
    public JwtSecurityToken Execute(IEnumerable<Claim> claims, IConfiguration config)
    {
        var key = config.GetSection("JWT").GetValue<string>("Key")
                ?? throw new InvalidOperationException(ResourceErrorMessages.ERROR_INVALID_SECRET_KEY);

        var privateKey = Encoding.UTF8.GetBytes(key);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),
            Audience = config.GetSection("JWT").GetValue<string>("ValidAudience"),
            Issuer = config.GetSection("JWT").GetValue<string>("ValidIssuer"),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return token;
    }
}
