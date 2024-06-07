using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrangeBranchTaskManager.Api.DTOs;
using OrangeBranchTaskManager.Api.Services;

namespace OrangeBranchTaskManager.Api.Controllers
{
    // Tomei a liberdade de adicionar mais códigos de status para outras situações

    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private ITasksService _taskService;

        public TasksController(ITasksService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
        {
            try
            {
                var tasks = await _taskService.GetTasks();

                if (tasks is null) return BadRequest();

                return Ok(tasks);
            }
            catch (Exception)
            {
                return BadRequest("Erro ao buscar tarefas");
            }
        }

        [HttpGet("GetTask")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TaskDTO>> GetTask([FromQuery] int id)
        {
            try
            {
                var task = await _taskService.GetById(id);

                if (task is null) return BadRequest();

                return Ok(task);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Erro ao buscar tarefa");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Tarefa não encontrada");
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDTO>> CreateTask(TaskDTO taskData)
        {
            try
            {
                var newTask = await _taskService.CreateTask(taskData);

                if (newTask is null) return BadRequest();

                return CreatedAtAction("CreateTask", newTask);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Erro ao criar tarefa");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Erro ao salvar tarefa no banco de dados");
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDTO>> UpdateTask([FromQuery] int id, TaskDTO taskData)
        {
            try
            {
                var updatedTask = await _taskService.UpdateTask(id, taskData);

                if (updatedTask is null) return BadRequest();

                return Ok(updatedTask);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Erro ao atualizar a tarefa");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Tarefa não encontrada");
            }
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTask([FromQuery] int id)
        {
            try
            {
                await _taskService.DeleteTask(id);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Erro ao excluir a tarefa");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Tarefa não encontrada");
            }
        }
    }
}
