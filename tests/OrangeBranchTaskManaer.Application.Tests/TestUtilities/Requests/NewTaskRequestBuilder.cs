using Bogus;
using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManaer.Application.Tests.TestUtilities.Requests;

public class NewTaskRequestBuilder
{
    public static TaskDTO Build()
    {
        return new Faker<TaskDTO>()
            .RuleFor(r => r.Title, faker => faker.Lorem.Sentence(2))
            .RuleFor(r => r.Description, faker => faker.Lorem.Sentence(5))
            .RuleFor(r => r.DueDate, faker => faker.Date.Future());
    }
}
