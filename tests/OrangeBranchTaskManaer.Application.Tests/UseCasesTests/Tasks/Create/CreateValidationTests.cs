using FluentAssertions;
using OrangeBranchTaskManaer.Application.Tests.TestUtilities.Requests;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Create;

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
}
