using AutoMapper;
using FluentAssertions;
using Moq;
using OrangeBranchTaskManager.Api.Controllers.Mappings;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Update;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Tasks.Update;

public class UpdateTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ISendEmailUseCase> _sendEmailUseCaseMock;

    public UpdateTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _taskRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TaskModel { Id = 1, Title = "Lorem Ipsum"});
        _taskRepositoryMock.Setup(repository => repository.UpdateAsync(It.IsAny<TaskModel>()))
            .Returns(new TaskModel { Id = 1, Title = "Lorem Ipsum"});
        _unitOfWorkMock.Setup(uow => uow.TaskRepository).Returns(_taskRepositoryMock.Object);
        _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

        var mapperConfig = new MapperConfiguration(config =>
        {
            config.AddProfile<TaskDTOMappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _sendEmailUseCaseMock = new Mock<ISendEmailUseCase>();
    }

    [Fact]
    public async Task Success()
    {
        var updateTask = new UpdateTaskUseCase(
            _unitOfWorkMock.Object, 
            _mapper, 
            _sendEmailUseCaseMock.Object);
        var request = UpdateTaskRequestBuilder.Build();
        var updateId = request.Id;

        var result = await updateTask.Execute(updateId, request);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(TaskDTO));
    }

    [Fact]
    public async Task Should_Throw_Exception_On_Mismatched_Id()
    {
        var updateTask = new UpdateTaskUseCase(
            _unitOfWorkMock.Object, 
            _mapper, 
            _sendEmailUseCaseMock.Object);
        var request = UpdateTaskRequestBuilder.Build();
        var updateId = request.Id + 1;

        Func<Task> result = () => updateTask.Execute(updateId, request);

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();

        errors.Should().ContainKey(nameof(TaskDTO.Id))
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_ID_DOESNT_MATCH);
    }

    [Fact]
    public async Task Should_Throw_Exception_On_Task_Not_Found()
    {
        _taskRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))!
            .ReturnsAsync((TaskModel)null!);
        
        var updateTask = new UpdateTaskUseCase(
            _unitOfWorkMock.Object, 
            _mapper, 
            _sendEmailUseCaseMock.Object);
        var request = UpdateTaskRequestBuilder.Build();
        var updateId = request.Id;

        Func<Task> result = () => updateTask.Execute(updateId, request);

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();

        errors.Should().ContainKey(ResourceErrorMessages.ERROR)
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);
    }
}
