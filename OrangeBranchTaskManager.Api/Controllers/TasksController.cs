using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrangeBranchTaskManager.Api.DTOs;
using OrangeBranchTaskManager.Api.Services;

namespace OrangeBranchTaskManager.Api.Controllers
{

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
        {
            var tasks = await _taskService.GetTasks();

            if (tasks is null) return BadRequest();

            return Ok(tasks);
        }

        [HttpGet("id:int", Name = "GetTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDTO>> GetTask(int id)
        {
            var task = await _taskService.GetById(id);

            if (task is null) return BadRequest();

            return Ok(task);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDTO>> CreateTask(TaskDTO taskData)
        {
            var newTask = await _taskService.CreateTask(taskData);

            if (newTask is null) return BadRequest();

            return Ok(newTask);
        }
    }
}
