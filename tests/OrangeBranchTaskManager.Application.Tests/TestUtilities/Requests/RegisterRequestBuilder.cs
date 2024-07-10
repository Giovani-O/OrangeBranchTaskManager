using Bogus;
using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.Tests.TestUtilities.Requests;

public class RegisterRequestBuilder
{
    public static RegisterDTO Build()
    {
        return new Faker<RegisterDTO>()
            .RuleFor(r => r.Username, faker => faker.Internet.UserName())
            .RuleFor(r => r.Email, faker => faker.Internet.Email())
            .RuleFor(r => r.Password, faker => faker.Internet.Password());
    }
}