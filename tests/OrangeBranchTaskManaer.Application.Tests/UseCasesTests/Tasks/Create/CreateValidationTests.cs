using Bogus;
using FluentAssertions;
using Microsoft.VisualBasic;
using OrangeBranchTaskManaer.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManaer.Application.Tests.TestUtilities.Tools;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Create;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManaer.Application.Tests.UseCasesTests.Tasks.Create;

public class CreateValidationTests
{
    [Fact]
    public void Success()
    {
        var validator = new CreateTaskValidator();
        var request = NewTaskRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        var validator = new CreateTaskValidator();
        var request = NewTaskRequestBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_TITLE_EMPTY));
    }

    [Fact]
    public void Error_Title_Too_Long()
    {
        var validator = new CreateTaskValidator();
        var request = NewTaskRequestBuilder.Build();
        request.Title = StringGenerator.NewString(110);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_TITLE_TOO_LONG));
    }

    [Theory]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData(null)]
    public void Error_Description_Empty(string description)
    {
        var validator = new CreateTaskValidator();
        var request = NewTaskRequestBuilder.Build();
        request.Description = description;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DESCRIPTION_EMPTY));
    }

    [Fact]
    public void Error_Description_Too_Long()
    {
        var validator = new CreateTaskValidator();
        var request = NewTaskRequestBuilder.Build();
        request.Description = StringGenerator.NewString(310);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DESCRIPTION_TOO_LONG));
    }

    [Fact]
    public void Error_DueDate_Empty()
    {
        var validator = new CreateTaskValidator();
        var request = NewTaskRequestBuilder.Build();
        request.DueDate = default;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DUE_DATE_EMPTY));
    }

    [Fact]
    public void Error_DueDate_In_The_Past()
    {
        var validator = new CreateTaskValidator();
        var request = NewTaskRequestBuilder.Build();
        request.DueDate = DateTime.UtcNow.AddMonths(-1);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DUE_DATE_PAST));
    }
}
