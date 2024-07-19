using FluentAssertions;
using Moq;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Tools;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.PublishMessage;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using RabbitMQ.Client;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.SendEmail;

public class SendEmailTests
{
    private readonly Mock<IRabbitMQConnectionManager> _connectionManagerMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<SendEmailUseCase> _sendEmailUseCase;

    public SendEmailTests()
    {
        _connectionManagerMock = new Mock<IRabbitMQConnectionManager>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _sendEmailUseCase = new Mock<SendEmailUseCase>(_connectionManagerMock.Object, _currentUserServiceMock.Object);
        
        _connectionManagerMock.Setup(x => x.GetChannel()).Returns(Mock.Of<IModel>());
        _currentUserServiceMock.Setup(x => x.GetUsername()).Returns(StringGenerator.NewString(8));
        _currentUserServiceMock.Setup(x => x.GetEmail()).Returns(StringGenerator.NewEmail());
    }

    [Fact]
    public async Task DeleteTask_Success()
    {
        var titleMock = StringGenerator.NewString(8);

        await _sendEmailUseCase.Invoking(async x => await x.Object.DeleteTaskExecute(titleMock))
            .Should().NotThrowAsync();
    }

    [Fact]
    public async Task CreateTask_Success()
    {
        var taskDtoMock = NewTaskRequestBuilder.Build();

        await _sendEmailUseCase.Invoking(async x => await x.Object.CreateTaskExecute(taskDtoMock))
            .Should().NotThrowAsync();
    }

    [Fact]
    public async Task UpdateTask_Success()
    {
        var taskDtoMock = NewTaskRequestBuilder.Build();

        await _sendEmailUseCase.Invoking(async x => await x.Object.UpdateTaskExecute(taskDtoMock))
            .Should().NotThrowAsync();
    }
}