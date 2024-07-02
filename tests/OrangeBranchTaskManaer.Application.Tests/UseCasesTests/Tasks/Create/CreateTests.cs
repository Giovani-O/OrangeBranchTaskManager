using AutoMapper;
using FluentAssertions;
using Moq;
using OrangeBranchTaskManaer.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Api.Controllers.Mappings;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Create;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManaer.Application.Tests.UseCasesTests.Tasks.Create;

public class CreateTests
{

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ISendEmailUseCase> _sendEmailUseCaseMock;

    public CreateTests()
    {
        // Unit of work and repository
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _taskRepositoryMock.Setup(repository => repository.CreateAsync(It.IsAny<TaskModel>()))
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
        var createTask = new CreateTaskUseCase(
            _unitOfWorkMock.Object,
            _mapper,
            _sendEmailUseCaseMock.Object
        );
        var request = NewTaskRequestBuilder.Build();

        var result = await createTask.Execute(request);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(TaskDTO));
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Should_Throw_ErrorOnExecutionException()
    {
        _taskRepositoryMock.Setup(repository => repository.CreateAsync(It.IsAny<TaskModel>()))
            .Returns((TaskModel)null);

        var createTask = new CreateTaskUseCase(
            _unitOfWorkMock.Object,
            _mapper,
            _sendEmailUseCaseMock.Object
        );
        var request = NewTaskRequestBuilder.Build();

        Func<Task> result = async () => await createTask.Execute(request);

        result.Should().ThrowAsync<ErrorOnExecutionException>();
    }
}
