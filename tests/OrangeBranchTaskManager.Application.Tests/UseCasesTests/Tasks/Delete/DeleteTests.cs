using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using OrangeBranchTaskManager.Api.Controllers.Mappings;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Tasks.Delete;

public class DeleteTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<ISendEmailUseCase> _sendEmailUseCaseMock;

    public DeleteTests()
    {
        // Unit of work and repository
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        
        _taskRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TaskModel { Id = 1, Title = "Lorem Ipsum"});
        _taskRepositoryMock.Setup(repository => repository.DeleteAsync(It.IsAny<TaskModel>()))
            .Returns(new TaskModel { Id = 1 });
        
        _unitOfWorkMock.Setup(uow => uow.TaskRepository).Returns(_taskRepositoryMock.Object);
        _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

        // AutoMapper
        var mapperConfig = new MapperConfiguration(
            config =>
            {
                config.AddProfile<TaskDTOMappingProfile>();
            }
        );
        _mapper = mapperConfig.CreateMapper();
        
        // Email sender
        _sendEmailUseCaseMock = new Mock<ISendEmailUseCase>();
    }

    [Fact]
    public async Task Success()
    {
        var deleteTask = new DeleteTaskUseCase(
            _unitOfWorkMock.Object, 
            _mapper, 
            _sendEmailUseCaseMock.Object);
        var faker = new Faker();
        var request = faker.Random.Int(1, 100);

        var result = await deleteTask.Execute(request);
        
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(TaskDTO));
        result.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task Should_Throw_ErrorOnExecutionException()
    {
        _taskRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))!
            .ReturnsAsync((TaskModel)null);
        
        var deleteTask = new DeleteTaskUseCase(
            _unitOfWorkMock.Object, 
            _mapper, 
            _sendEmailUseCaseMock.Object);
        var faker = new Faker();
        var request = faker.Random.Int(1, 100);

        Func<Task> result = async () => await deleteTask.Execute(request);
        
        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();

        errors.Should().ContainKey(ResourceErrorMessages.ERROR)
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_DELETE_TASK);
    }
}
