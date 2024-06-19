using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrangeBranchTaskManager.Api.Services
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration config);
    }
}
