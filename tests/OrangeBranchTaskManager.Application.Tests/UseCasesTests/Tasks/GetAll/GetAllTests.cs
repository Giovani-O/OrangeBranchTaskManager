using System.Diagnostics;
using AutoMapper;
using FluentAssertions;
using Moq;
using OrangeBranchTaskManager.Api.Controllers.Mappings;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetAll;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Tasks.GetAll;

public class GetAllTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly IMapper _mapper;

    public GetAllTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        var tasks = new List<TaskModel>
        {
            new TaskModel { Id = 1, Title = "Lorem Ipsum", Description = "Lorem Ipsum" },
            new TaskModel { Id = 2, Title = "Dolor Sit Amet", Description = "Dolor Sit Amet" },
        };
        _taskRepositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(tasks);
        _unitOfWorkMock.Setup(uow => uow.TaskRepository).Returns(_taskRepositoryMock.Object);

        var mapperConfig = new MapperConfiguration(
            config =>
            {
                config.AddProfile<TaskDTOMappingProfile>();
            }
        );
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task Success()
    {
        var getAllTasks = new GetAllTasksUseCase(
            _unitOfWorkMock.Object,
            _mapper);

        var result = await getAllTasks.Execute();
        var taskDtos = result as TaskDTO[] ?? result.ToArray();
        
        taskDtos.Should().NotBeNull();
        taskDtos.Should().NotBeEmpty();
        taskDtos.Should().BeOfType<TaskDTO[]>();
        taskDtos.Should().HaveCount(2);
    }

    [Fact]
    public async Task Should_Throw_Exception_On_Tasks_Not_Found()
    {
        var tasks = new List<TaskModel> { };
        _taskRepositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(tasks);
        
        var getAllTasks = new GetAllTasksUseCase(
            _unitOfWorkMock.Object,
            _mapper);
        
        Func<Task> result = async () => await getAllTasks.Execute();

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();

        errors.Should().ContainKey(ResourceErrorMessages.ERROR)
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_NOT_FOUND_TASKS);
    }
}
