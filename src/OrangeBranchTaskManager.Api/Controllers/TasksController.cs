using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Create;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetAll;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Update;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;

namespace OrangeBranchTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISendEmailUseCase _sendEmailUseCase;

        public TasksController(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ISendEmailUseCase sendEmailUseCase
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sendEmailUseCase = sendEmailUseCase;
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TaskDTO>> CreateTask(TaskDTO taskData)
        {
            var useCase = new CreateTaskUseCase(_unitOfWork, _mapper, _sendEmailUseCase);
            var response = await useCase.Execute(taskData);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDTO>> UpdateTask([FromQuery] int id, TaskDTO taskData)
        {
            var useCase = new UpdateTaskUseCase(_unitOfWork, _mapper, _sendEmailUseCase);
            var response = await useCase.Execute(id, taskData);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTask([FromQuery] int id)
        {
            var useCase = new DeleteTaskUseCase(_unitOfWork, _mapper, _sendEmailUseCase);
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
