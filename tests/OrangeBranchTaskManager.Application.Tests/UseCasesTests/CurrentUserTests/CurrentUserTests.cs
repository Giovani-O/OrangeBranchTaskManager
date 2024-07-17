using System.Security.Claims;
using System.Security.Principal;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Tools;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.CurrentUserTests;

public class CurrentUserTests
{
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<ClaimsPrincipal> _userMock;

    public CurrentUserTests()
    {
        // Inicia mocks
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _userMock = new Mock<ClaimsPrincipal>();
        Mock<HttpContext> httpContextMock = new();
        Mock<IIdentity> identityMock = new();
        
        // Configuração de IsAuthenticated e Identity
        identityMock.Setup(identity => identity.IsAuthenticated).Returns(true);
        _userMock.Setup(user => user.Identity).Returns(identityMock.Object);

        // Configuração da claim
        var claim = new Claim(ClaimTypes.Name, StringGenerator.NewString(8));
        _userMock.Setup(user => user.FindFirst(It.IsAny<string>())).Returns(claim);
        
        // Configuração do User e HttpContext
        httpContextMock.Setup(context => context.User).Returns(_userMock.Object);
        _contextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContextMock.Object);
    }
    
    [Fact]
    public void GetUsername_Success()
    {
        var service = new CurrentUserService(_contextAccessorMock.Object);

        var result = service.GetUsername();

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }
    
    [Fact]
    public void GetEmail_Success()
    {
        var faker = new Faker();
        var claim = new Claim(ClaimTypes.Email, faker.Internet.Email());
        _userMock.Setup(user => user.FindFirst(It.IsAny<string>())).Returns(claim);
        
        var service = new CurrentUserService(_contextAccessorMock.Object);

        var result = service.GetEmail();

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }
}