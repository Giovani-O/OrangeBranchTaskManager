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
    private readonly Mock<IConfigurationSection> _tokenValidityInMinutesSectionMock;
    private readonly Mock<IConfigurationSection> _validAudienceSectionMock;
    private readonly Mock<IConfigurationSection> _validIssuerSectionMock;
    private readonly Mock<IConfigurationSection> _jwtSectionMock;

    public TokenServiceUnitTests()
    {
        // Inicia mocks
        _configurationMock = new Mock<IConfiguration>();
        _keySectionMock = new Mock<IConfigurationSection>(); 
        _tokenValidityInMinutesSectionMock = new Mock<IConfigurationSection>();
        _validAudienceSectionMock = new Mock<IConfigurationSection>();
        _validIssuerSectionMock = new Mock<IConfigurationSection>();
        _jwtSectionMock = new Mock<IConfigurationSection>();

        // Atribui valores válidos
        _keySectionMock.Setup(config => config.Value).Returns(StringGenerator.NewString(32));
        _tokenValidityInMinutesSectionMock.Setup(config => config.Value).Returns("60");
        _validAudienceSectionMock.Setup(config => config.Value).Returns("AudienceMock");
        _validIssuerSectionMock.Setup(config => config.Value).Returns("IssuerMock");
        
        // Configura GetSection
        _jwtSectionMock.Setup(config => config.GetSection("Key")).Returns(_keySectionMock.Object);
        _jwtSectionMock.Setup(config => config.GetSection("TokenValidityInMinutes"))
            .Returns(_tokenValidityInMinutesSectionMock.Object);
        _jwtSectionMock.Setup(config => config.GetSection("ValidAudience")).Returns(_validAudienceSectionMock.Object);
        _jwtSectionMock.Setup(config => config.GetSection("ValidIssuer")).Returns(_validIssuerSectionMock.Object);
        
        _configurationMock.Setup(config => config.GetSection("JWT")).Returns(_jwtSectionMock.Object);
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
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Email, "test@test.com"),
            new Claim("id", "TestUser"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        Action result = () => useCase.Execute(claims, _configurationMock.Object);

        var exception = result.Should().Throw<ErrorOnExecutionException>();
        var errors = exception.Which.GetErrors();
        
        errors.Should().ContainKey(ResourceErrorMessages.ERROR)
            .WhoseValue.Should().Contain(ResourceErrorMessages.ERROR_INVALID_SECRET_KEY);
    }
}