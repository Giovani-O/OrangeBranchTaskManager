using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Tools;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.TokenService;

public class TokenServiceUnitTests
{
    private readonly Mock<IConfiguration> _configurationMock;

    public TokenServiceUnitTests()
    {
        // Inicia mocks
        _configurationMock = new Mock<IConfiguration>();
        Mock<IConfigurationSection> jwtSectionMock = new();
        Mock<IConfigurationSection> keySectionMock = new();
        Mock<IConfigurationSection> tokenValidityInMinutesSectionMock = new();
        Mock<IConfigurationSection> validAudienceSectionMock = new();
        Mock<IConfigurationSection> validIssuerSectionMock = new();

        // Atribui valores válidos
        keySectionMock.Setup(config => config.Value).Returns(StringGenerator.NewString(32));
        tokenValidityInMinutesSectionMock.Setup(config => config.Value).Returns("60");
        validAudienceSectionMock.Setup(config => config.Value).Returns("AudienceMock");
        validIssuerSectionMock.Setup(config => config.Value).Returns("IssuerMock");
        
        // Configura GetSection
        jwtSectionMock.Setup(config => config.GetSection("Key")).Returns(keySectionMock.Object);
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
}