using FluentAssertions;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.Tests.TestUtilities.Tools;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Update;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Tasks.Update;

public class UpdateValidationTests
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Error_Invalid_Id(int id)
    {
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();
        request.Id = id;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_INVALID_ID));
    }

    [Theory]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_TITLE_EMPTY));
    }

    [Fact]
    public void Error_Title_Too_Long()
    {
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();
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
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();
        request.Description = description;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DESCRIPTION_EMPTY));
    }

    [Fact]
    public void Error_Description_Too_Long()
    {
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();
        request.Description = StringGenerator.NewString(310);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DESCRIPTION_TOO_LONG));
    }

    [Fact]
    public void Error_DueDate_Empty()
    {
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();
        request.DueDate = default;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DUE_DATE_EMPTY));
    }

    [Fact]
    public void Error_DueDate_In_The_Past()
    {
        var validator = new UpdateTaskValidator();
        var request = UpdateTaskRequestBuilder.Build();
        request.DueDate = DateTime.UtcNow.AddMonths(-1);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.ERROR_DUE_DATE_PAST));
    }
}