using Bogus;
using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;

public class LoginRequestBuilder
{
    public static LoginDTO Build()
    {
        return new Faker<LoginDTO>()
            .RuleFor(r => r.Email, faker => faker.Internet.Email())
            .RuleFor(r => r.Password, faker => faker.Internet.Password());
    }
}