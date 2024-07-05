using FluentAssertions;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Tasks.GetById;

public class GetByIdValidationTests
{
    [Fact]
    public void Success()
    {
        var validator = new GetTaskByIdValidator();
        var random = new Random();
        var randomId = random.Next(1, int.MaxValue);

        var result = validator.Validate(randomId);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Failure()
    {
        var validator = new GetTaskByIdValidator();
        var random = new Random();
        var randomId = random.Next(int.MinValue, 0);

        var result = validator.Validate(randomId);

        result.IsValid.Should().BeFalse();
    }
}