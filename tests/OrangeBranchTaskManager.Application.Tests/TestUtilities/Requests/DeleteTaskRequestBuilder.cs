using Bogus;
using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;

public class DeleteTaskRequestBuilder
{
    public static TaskDTO Build()
    {
        return new Faker<TaskDTO>()
            .RuleFor(r => r.Id, faker => faker.Random.Int(1, 100));
    }
}