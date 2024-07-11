using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;

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
}