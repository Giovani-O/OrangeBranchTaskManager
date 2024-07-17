using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Tools;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.TokenService;

public class TokenServiceUnitTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IConfigurationSection> _keySectionMock;

    public TokenServiceUnitTests()
    {
        // Inicia mocks
        _configurationMock = new Mock<IConfiguration>();
        _keySectionMock = new Mock<IConfigurationSection>(); 
        Mock<IConfigurationSection> tokenValidityInMinutesSectionMock = new();
        Mock<IConfigurationSection> validAudienceSectionMock = new();
        Mock<IConfigurationSection> validIssuerSectionMock = new();
        Mock<IConfigurationSection> jwtSectionMock = new();

        // Atribui valores válidos
        _keySectionMock.Setup(config => config.Value).Returns(StringGenerator.NewString(32));
        tokenValidityInMinutesSectionMock.Setup(config => config.Value).Returns("60");
        validAudienceSectionMock.Setup(config => config.Value).Returns("AudienceMock");
        validIssuerSectionMock.Setup(config => config.Value).Returns("IssuerMock");
        
        // Configura GetSection
        jwtSectionMock.Setup(config => config.GetSection("Key")).Returns(_keySectionMock.Object);
        jwtSectionMock.Setup(config => config.GetSection("TokenValidityInMinutes"))
            .Returns(tokenValidityInMinutesSectionMock.Object);
        jwtSectionMock.Setup(config => config.GetSection("ValidAudience")).Returns(validAudienceSectionMock.Object);
        jwtSectionMock.Setup(config => config.GetSection("ValidIssuer")).Returns(validIssuerSectionMock.Object);
        
        _configurationMock.Setup(config => config.GetSection("JWT")).Returns(jwtSectionMock.Object);
    }
    
    [Fact]
    public void Success()
    {
        var useCase = new TokenServiceUseCase();
        var faker = new Faker();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, faker.Internet.UserName()),
            new Claim(ClaimTypes.Email, faker.Internet.Email()),
            new Claim("id", faker.Internet.UserName()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var result = useCase.Execute(claims, _configurationMock.Object);
        
        result.Should().NotBeNull();
        result.Should().BeOfType<JwtSecurityToken>();
    }

    [Fact]
    public void Should_Throw_Exception_On_Invalid_Secret_Key()
    { 
        _keySectionMock.Setup(config => config.Value).Returns((string)null!);
        
        var useCase = new TokenServiceUseCase();
        var faker = new Faker();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, faker.Internet.UserName()),
            new Claim(ClaimTypes.Email, faker.Internet.Email()),
            new Claim("id", faker.Internet.UserName()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        Action result = () => useCase.Execute(claims, _configurationMock.Object);

        var exception = result.Should().Throw<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();
        
        errors.Should().ContainKey(ResourceErrorMessages.ERROR)
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_INVALID_SECRET_KEY);
    }
}