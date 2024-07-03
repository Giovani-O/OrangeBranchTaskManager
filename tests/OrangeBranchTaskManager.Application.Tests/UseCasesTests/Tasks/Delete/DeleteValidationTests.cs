using FluentAssertions;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;

namespace OrangeBranchTaskManager.Application.Tests.UseCasesTests.Tasks.Delete;

public class DeleteValidationTests
{
    [Fact]
    public void Success()
    {
        var validator = new DeleteTaskValidator();
        var random = new Random();
        var randomId = random.Next(1, int.MaxValue);

        var result = validator.Validate(randomId);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Failure()
    {
        var validator = new DeleteTaskValidator();
        var random = new Random();
        var randomId = random.Next(int.MinValue, 0);

        var result = validator.Validate(randomId);

        result.IsValid.Should().BeFalse();
    }
}