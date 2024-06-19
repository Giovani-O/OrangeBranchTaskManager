using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrangeBranchTaskManager.Application.UseCases.Task;
using OrangeBranchTaskManager.Application.UseCases.Task.Create;
using OrangeBranchTaskManager.Application.UseCases.Task.Delete;
using OrangeBranchTaskManager.Application.UseCases.Task.GetAll;
using OrangeBranchTaskManager.Application.UseCases.Task.GetById;
using OrangeBranchTaskManager.Application.UseCases.Task.Update;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;

namespace OrangeBranchTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TasksController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
        {
            var useCase = new GetAllTasksUseCase(_unitOfWork, _mapper);
            var response = await useCase.Execute();

            return Ok(response);
        }

        [HttpGet("GetTask")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TaskDTO>> GetTask([FromQuery] int id)
        {
            var useCase = new GetTaskByIdUseCase(_unitOfWork, _mapper);
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDTO>> CreateTask(TaskDTO taskData)
        {
            var useCase = new CreateTaskUseCase(_unitOfWork, _mapper);
            var response = await useCase.Execute(taskData);

            return Ok(response);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDTO>> UpdateTask([FromQuery] int id, TaskDTO taskData)
        {
            var useCase = new UpdateTaskUseCase(_unitOfWork, _mapper);
            var response = await useCase.Execute(id, taskData);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTask([FromQuery] int id)
        {
            var useCase = new DeleteTaskUseCase(_unitOfWork, _mapper);
            var response = await useCase.Execute(id);

            return Ok(response);
        }
    }


    //[Route("api/[controller]")]
    //[ApiController]
    //public class TasksController : ControllerBase
    //{
    //    private ITaskUseCases _taskUseCases;

    //    public TasksController(ITaskUseCases taskUseCases)
    //    {
    //        _taskUseCases = taskUseCases;
    //    }

    //    [HttpGet]
    //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
    //    {
    //        try
    //        {
    //            var tasks = await _taskUseCases.GetTasks();

    //            if (tasks is null) return BadRequest();

    //            return Ok(tasks);
    //        }
    //        catch (System.Exception)
    //        {
    //            return BadRequest(ResourceErrorMessages.ERROR_BAD_REQUEST_TASKS);
    //        }
    //    }

    //    [HttpGet("GetTask")]
    //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //    public async Task<ActionResult<TaskDTO>> GetTask([FromQuery] int id)
    //    {
    //        try
    //        {
    //            var task = await _taskUseCases.GetById(id);

    //            if (task is null) return BadRequest();

    //            return Ok(task);
    //        }
    //        catch (ArgumentNullException)
    //        {
    //            return BadRequest(ResourceErrorMessages.ERROR_BAD_REQUEST_TASK);
    //        }
    //        catch (KeyNotFoundException)
    //        {
    //            return NotFound(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);
    //        }
    //    }

    //    [HttpPost]
    //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //    [ProducesResponseType(StatusCodes.Status201Created)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    public async Task<ActionResult<TaskDTO>> CreateTask(TaskDTO taskData)
    //    {
    //        try
    //        {
    //            var newTask = await _taskUseCases.CreateTask(taskData);

    //            if (newTask is null) return BadRequest();

    //            return CreatedAtAction("CreateTask", newTask);
    //        }
    //        catch (ArgumentNullException)
    //        {
    //            return BadRequest(ResourceErrorMessages.ERROR_BAD_REQUEST_TASK);
    //        }
    //        catch (DbUpdateException)
    //        {
    //            return BadRequest(ResourceErrorMessages.ERROR_SAVE_TASK_DB);
    //        }
    //    }

    //    [HttpPut]
    //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public async Task<ActionResult<TaskDTO>> UpdateTask([FromQuery] int id, TaskDTO taskData)
    //    {
    //        try
    //        {
    //            var updatedTask = await _taskUseCases.UpdateTask(id, taskData);

    //            if (updatedTask is null) return BadRequest();

    //            return Ok(updatedTask);
    //        }
    //        catch (ArgumentNullException)
    //        {
    //            return BadRequest(ResourceErrorMessages.ERROR_UPDATE_TASK);
    //        }
    //        catch (KeyNotFoundException)
    //        {
    //            return NotFound(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);
    //        }
    //    }

    //    [HttpDelete]
    //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //    [ProducesResponseType(StatusCodes.Status204NoContent)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public async Task<ActionResult> DeleteTask([FromQuery] int id)
    //    {
    //        try
    //        {
    //            await _taskUseCases.DeleteTask(id);
    //            return NoContent();
    //        }
    //        catch (ArgumentNullException)
    //        {
    //            return BadRequest(ResourceErrorMessages.ERROR_DELETE_TASK);
    //        }
    //        catch (KeyNotFoundException)
    //        {
    //            return NotFound(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);
    //        }
    //    }
    //}
}
