using Microsoft.AspNetCore.Mvc;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginData, [FromServices] ILoginUseCase useCase)
        {
            var result = await useCase.Execute(loginData);

            return Ok(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerData, [FromServices] IRegisterUseCase useCase)
        {
            var result = await useCase.Execute(registerData);

            return Ok(result);
        }
    }
}
