using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using OrangeBranchTaskManager.Api.DTOs;
using OrangeBranchTaskManager.Api.Models;
using OrangeBranchTaskManager.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrangeBranchTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> _userManager;

        public AuthenticationController(
            ITokenService tokenService, 
            IConfiguration configuration, 
            UserManager<UserModel> userManager, 
            RoleManager<IdentityRole> roleManager
        )
        {
            _tokenService = tokenService;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, loginData.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("id", user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                await _userManager.UpdateAsync(user);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                var tokenExpiration = new JwtSecurityToken(jwtToken).ValidTo;

                // Armazena token nos cookies
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = tokenExpiration
                };
                Response.Cookies.Append("token-string", jwtToken, cookieOptions);

                return Ok(new {
                    Token = jwtToken,
                    ValidTo = tokenExpiration
                });
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerData)
        {
            var existentUser = await _userManager.FindByEmailAsync(registerData.Email!);
            if (existentUser is not null) return Conflict("Já existe um usuário com o email informado");

            UserModel user = new()
            {
                UserName = registerData.Username,
                Email = registerData.Email,
                NormalizedEmail = _userManager.NormalizeEmail(registerData.Email)
        };

            var result = await _userManager.CreateAsync(user, registerData.Password!);

            if (!result.Succeeded) return BadRequest("Não foi possível criar usuário");

            return Ok(result);
        }
    }
}
