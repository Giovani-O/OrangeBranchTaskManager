using FluentAssertions;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Tools;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Authentication.Register;

public class RegisterValidationTests
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Error_Username_Empty(string username)
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        task.Username = username;
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
            .Contain(e => e.PropertyName == "Username" && e.ErrorMessage == ResourceErrorMessages.ERROR_USERNAME_EMPTY);
    }

    [Fact]
    public void Error_Username_Too_Long()
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        task.Username = StringGenerator.NewString(101);
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
           .Contain(e => e.PropertyName == "Username" && e.ErrorMessage == ResourceErrorMessages.ERROR_USERNAME_TOO_LONG);
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    public void Error_Email_Empty(string email)
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        task.Email = email;
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
           .Contain(e => e.PropertyName == "Email" && e.ErrorMessage == ResourceErrorMessages.ERROR_EMAIL_EMPTY);
    }

    [Theory]
    [InlineData("email.email.com")]
    [InlineData("invalid_email")]
    public void Error_Invalid_Email(string email)
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        task.Email = email;
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
           .Contain(e => e.PropertyName == "Email" && e.ErrorMessage == ResourceErrorMessages.ERROR_INVALID_EMAIL);
    }

    [Fact]
    public void Error_Email_Too_Long()
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        task.Email = $"{StringGenerator.NewString(51)}@{StringGenerator.NewString(50)}.com";
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
           .Contain(e => e.PropertyName == "Email" && e.ErrorMessage == ResourceErrorMessages.ERROR_EMAIL_TOO_LONG);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Error_Password_Empty(string password)
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        task.Password = password;
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
           .Contain(e => e.PropertyName == "Password" && e.ErrorMessage == ResourceErrorMessages.ERROR_PASSWORD_EMPTY);
    }

    [Fact]
    public void Error_Password_Too_Long()
    {
        var validator = new RegisterValidator();
        var task = RegisterRequestBuilder.Build();
        task.Password = StringGenerator.NewString(101);
        
        var validationResult = validator.Validate(task);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
           .Contain(e => e.PropertyName == "Password" && e.ErrorMessage == ResourceErrorMessages.ERROR_PASSWORD_TOO_LONG);
    }
}