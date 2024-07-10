using Bogus;
using FluentAssertions;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Authentication.Login;

public class LoginValidationTests
{
    [Fact]
    public void Success()
    {
        var validator = new LoginValidator();
        var request = LoginRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Error_Email_Empty(string email)
    {
        var validator = new LoginValidator();
        var request = LoginRequestBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_EMAIL_EMPTY));
    }

    [Theory]
    [InlineData("email.email.com")]
    [InlineData("invalid_email")]
    public void Error_Invalid_Email(string email)
    {
        var validator = new LoginValidator();
        var faker = new Faker();
        var request = LoginRequestBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_INVALID_EMAIL));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Error_Password_Empty(string password)
    {
        var validator = new LoginValidator();
        var request = LoginRequestBuilder.Build();
        request.Password = password;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_PASSWORD_EMPTY));
    }
}