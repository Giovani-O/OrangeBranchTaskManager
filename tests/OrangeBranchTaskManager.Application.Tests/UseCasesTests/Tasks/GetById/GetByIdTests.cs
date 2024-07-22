using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using OrangeBranchTaskManager.Application.Mappings;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Tasks.GetById;

public class GetByIdTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly IMapper _mapper;

    public GetByIdTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        var task = new TaskModel { Id = 1, Title = "Lorem Ipsum", Description = "Lorem Ipsum" };
        _taskRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(task);
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
        var getById = new GetTaskByIdUseCase(_unitOfWorkMock.Object, _mapper);
        var faker = new Faker();
        var id = faker.Random.Int(1, 100);

        var result = await getById.Execute(id);

        result.Should().NotBeNull();
        result.Should().BeOfType<TaskDTO>();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Throw_Exception_On_Task_Not_Found()
    {
        _taskRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))!
            .ReturnsAsync((TaskModel)null!);
        
        var getById = new GetTaskByIdUseCase(_unitOfWorkMock.Object, _mapper);
        var faker = new Faker();
        var id = faker.Random.Int(1, 100);

        Func<Task> result = async () => await getById.Execute(id);

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();
        
        errors.Should().ContainKey(ResourceErrorMessages.ERROR)
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);
    }
}
