using Emailing.Communication.Templates;

namespace Emailing.Application.UseCases.SendEmail;
public interface ISendEmailUseCase
{
    public void Execute(string message);
}
