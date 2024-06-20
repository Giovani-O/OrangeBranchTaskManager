using Microsoft.AspNetCore.Mvc;
using Producer.Application.UseCases.SendMessage;
using Producer.Communication.DTOs;
using Producer.Infrastructure.ConnectionManager;
using System.Text;

namespace Producer.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProducerController : ControllerBase
{
    private readonly IRabbitMQConnectionManager _connectionManager;

    public ProducerController(IRabbitMQConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(SendMessageDTO request)
    {
        var useCase = new SendMessageUseCase(_connectionManager);
        await useCase.Execute(request);

        return Ok("Mensagem enviada com sucesso!");
    }
}
