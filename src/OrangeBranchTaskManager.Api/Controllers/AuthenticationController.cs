using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrangeBranchTaskManager.Application.UseCases.Token;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Exception;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrangeBranchTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenServiceUseCase _tokenService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> _userManager;

        public AuthenticationController(
            ITokenServiceUseCase tokenService,
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

                var token = _tokenService.Execute(authClaims, _configuration);

                await _userManager.UpdateAsync(user);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                var tokenExpiration = new JwtSecurityToken(jwtToken).ValidTo;

                return Ok(new
                {
                    Token = jwtToken,
                    ValidTo = tokenExpiration,
                    UserName = user.UserName
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
            if (existentUser is not null) return Conflict(ResourceErrorMessages.ERROR_EMAIL_ALREADY_EXISTS);

            UserModel user = new()
            {
                UserName = registerData.Username,
                Email = registerData.Email,
                NormalizedEmail = _userManager.NormalizeEmail(registerData.Email)
            };

            var result = await _userManager.CreateAsync(user, registerData.Password!);

            if (!result.Succeeded) return BadRequest(ResourceErrorMessages.ERROR_CREATE_USER);

            return Ok(result);
        }
    }
}
