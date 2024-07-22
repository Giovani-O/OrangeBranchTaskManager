using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Create;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetAll;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Update;
using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks([FromServices] IGetAllTasksUseCase useCase)
        {
            var response = await useCase.Execute();

            return Ok(response);
        }

        [HttpGet("GetTask")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TaskDTO>> GetTask(
            [FromQuery] int id, 
            [FromServices] IGetTaskByIdUseCase useCase)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TaskDTO>> CreateTask(
            [FromBody] TaskDTO taskData, 
            [FromServices] ICreateTaskUseCase useCase)
        {
            var response = await useCase.Execute(taskData);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TaskDTO>> UpdateTask(
            [FromQuery] int id, 
            [FromBody] TaskDTO taskData, 
            [FromServices] IUpdateTaskUseCase useCase)
        {
            var response = await useCase.Execute(id, taskData);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteTask(
            [FromQuery] int id, 
            [FromServices] IDeleteTaskUseCase useCase)
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
