using Emailing.Application.UseCases.ConsumeMessage;
using Emailing.Domain.RabbitMQConnectionManager;
using Emailing.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IRabbitMQConnectionManager, RabbitMQConnectionManager>();
builder.Services.AddTransient<ConsumeMessageUseCase>();

var host = builder.Build();
host.Run();
