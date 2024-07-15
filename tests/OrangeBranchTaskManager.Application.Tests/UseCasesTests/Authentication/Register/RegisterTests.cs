using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using RabbitMQ.Client;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Authentication.Register;

public class RegisterTests
{
    private readonly Mock<ITokenServiceUseCase> _tokenServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<UserManager<UserModel>> _userManagerMock;
    private readonly Mock<IRabbitMQConnectionManager> _rabbitMQConnectionManager;

    public RegisterTests()
    {
        _tokenServiceMock = new Mock<ITokenServiceUseCase>();
        _configurationMock = new Mock<IConfiguration>();
        _userManagerMock = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _rabbitMQConnectionManager = new Mock<IRabbitMQConnectionManager>();
        
        // Setup de _tokenServiceMock.Execute
        _tokenServiceMock.Setup(service => service.Execute(
                It.IsAny<IEnumerable<Claim>>(), 
                It.IsAny<IConfiguration>()))
            .Returns(new JwtSecurityToken());
        
        // Setup de FindByEmailAsync
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(It.IsAny<String>()))
            .ReturnsAsync((UserModel)null!);
        
        // Setup de CreateAsync
        _userManagerMock.Setup(manager => manager.CreateAsync(
                It.IsAny<UserModel>(),
                It.IsAny<String>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Setup de ConnectionManager
        _rabbitMQConnectionManager.Setup(manager => manager.GetChannel())
           .Returns(Mock.Of<IModel>());
    }
    
    [Fact]
    public async Task Success()
    {
        var useCase = new RegisterUseCase(
            _tokenServiceMock.Object, 
            _configurationMock.Object, 
            _userManagerMock.Object, 
            _rabbitMQConnectionManager.Object);
        var request = RegisterRequestBuilder.Build();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(IdentityResult));
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Throw_Exception_On_Existing_User()
    {
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(It.IsAny<String>()))
            .ReturnsAsync(new UserModel { UserName = "JohnDoe", Email = "john@doe.com" });
        
        var useCase = new RegisterUseCase(
            _tokenServiceMock.Object, 
            _configurationMock.Object, 
            _userManagerMock.Object, 
            _rabbitMQConnectionManager.Object);
        var request = RegisterRequestBuilder.Build();

        Func<Task> result = () =>  useCase.Execute(request);

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();
        
        errors.Should().ContainKey(nameof(RegisterDTO.Email))
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_EMAIL_ALREADY_EXISTS);
    }

    [Fact]
    public async Task Should_Throw_Exception_On_Failed_Create()
    {
        _userManagerMock.Setup(manager => manager.CreateAsync(
                It.IsAny<UserModel>(),
                It.IsAny<String>()))
            .ReturnsAsync(IdentityResult.Failed());
        
        var useCase = new RegisterUseCase(
            _tokenServiceMock.Object, 
            _configurationMock.Object, 
            _userManagerMock.Object, 
            _rabbitMQConnectionManager.Object);
        var request = RegisterRequestBuilder.Build();

        Func<Task> result = () =>  useCase.Execute(request);

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();
        
        errors.Should().ContainKey(ResourceErrorMessages.ERROR)
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_CREATE_USER);
    }
}