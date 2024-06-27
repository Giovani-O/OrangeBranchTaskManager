using Emailing.Communication.Templates;

namespace Emailing.Application.UseCases.SendEmail;
public interface ISendEmailUseCase
{
    public Task Execute(string message);
}
