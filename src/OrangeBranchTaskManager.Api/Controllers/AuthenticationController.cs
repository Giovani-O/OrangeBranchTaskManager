using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;

namespace OrangeBranchTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenServiceUseCase _tokenService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> _userManager;
        private readonly IRabbitMQConnectionManager _connectionManager;

        public AuthenticationController(
            ITokenServiceUseCase tokenService,
            IConfiguration configuration,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            IRabbitMQConnectionManager connectionManager
        )
        {
            _tokenService = tokenService;
            _configuration = configuration;
            _userManager = userManager;
            _connectionManager = connectionManager;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginData)
        {
            var useCase = new LoginUseCase(_tokenService, _configuration, _userManager);
            var result = await useCase.Execute(loginData);

            return Ok(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerData)
        {
            var useCase = new RegisterUseCase(_tokenService, _configuration, _userManager, _connectionManager);
            var result = await useCase.Execute(registerData);

            return Ok(result);
        }
    }
}
