using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Authentication.Login;

public class LoginTests
{
    private readonly Mock<ITokenServiceUseCase> _tokenServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<UserManager<UserModel>> _userManagerMock;

    public LoginTests()
    {
        _tokenServiceMock = new Mock<ITokenServiceUseCase>();
        _configurationMock = new Mock<IConfiguration>();
        _userManagerMock = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        
        // Setup de _tokenServiceMock.Execute
        _tokenServiceMock.Setup(service => service.Execute(
                It.IsAny<IEnumerable<Claim>>(), 
                It.IsAny<IConfiguration>()))
            .Returns(new JwtSecurityToken());
        
        // Setup de FindByEmailAsync
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(It.IsAny<String>()))
            .ReturnsAsync(new UserModel { UserName = "JohnDoe", Email = "john@doe.com" });
        
        // Setup de CheckPasswordAsync
        _userManagerMock.Setup(manager => manager.CheckPasswordAsync(
                It.IsAny<UserModel>(), 
                It.IsAny<String>()))
            .ReturnsAsync(true);
        
        // Setup de GetRolesAsync
        _userManagerMock.Setup(manager => manager.GetRolesAsync(
                It.IsAny<UserModel>()))
            .ReturnsAsync(new List<string> { });
    }
    
    [Fact]
    public async Task Success()
    {
        var useCase = new LoginUseCase(
            _tokenServiceMock.Object, 
            _configurationMock.Object, 
            _userManagerMock.Object);
        var request = LoginRequestBuilder.Build();

        var result = await useCase.Execute(request);
        
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(LoginResponseDTO));
        result.Token.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Throw_Error_If_User_Not_Found()
    {
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(It.IsAny<String>()))
            .ReturnsAsync((UserModel)null!);
        
        var useCase = new LoginUseCase(
            _tokenServiceMock.Object, 
            _configurationMock.Object, 
            _userManagerMock.Object);
        var request = LoginRequestBuilder.Build();
        
        Func<Task> result = () => useCase.Execute(request);

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();

        errors.Should().ContainKey(nameof(LoginDTO.Email))
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_NOT_FOUND_USER);
    }

    [Fact]
    public async Task Should_Throw_Error_If_Invalid_Password()
    {
        _userManagerMock.Setup(manager => manager.CheckPasswordAsync(
                It.IsAny<UserModel>(), 
                It.IsAny<String>()))
            .ReturnsAsync(false);
        
        var useCase = new LoginUseCase(
            _tokenServiceMock.Object, 
            _configurationMock.Object, 
            _userManagerMock.Object);
        var request = LoginRequestBuilder.Build();
        
        Func<Task> result = () => useCase.Execute(request);

        var exception = await result.Should().ThrowAsync<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();

        errors.Should().ContainKey(nameof(LoginDTO.Password))
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_INVALID_PASSWORD);
    }
}