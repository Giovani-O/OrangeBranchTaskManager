using FluentAssertions;
using FluentValidation;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.SendEmail;

public class SendEmailValidationTests
{
    [Fact]
    public void Success()
    {   
        var validator = new SendEmailValidator();
        var task = NewTaskRequestBuilder.Build();

        var result = validator.Validate(task);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_On_Title_Empty()
    {
        var validator = new SendEmailValidator();
        var task = NewTaskRequestBuilder.Build();
        task.Title = "";

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_TITLE_EMPTY));
    }

    [Fact]
    public void Error_Description_Empty()
    {
        var validator = new SendEmailValidator();
        var task = NewTaskRequestBuilder.Build();
        task.Description = "";

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DESCRIPTION_EMPTY));
    }

    [Fact]
    public void Error_DueDate_Empty()
    {
        var validator = new SendEmailValidator();
        var task = NewTaskRequestBuilder.Build();
        task.DueDate = default;

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DUE_DATE_EMPTY));
    }

    [Fact]
    public void Error_DueDate_In_The_Past()
    {
        var validator = new SendEmailValidator();
        var task = NewTaskRequestBuilder.Build();
        task.DueDate = DateTime.UtcNow.AddMonths(-1);

        var result = validator.Validate(task);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DUE_DATE_PAST));
    }
}