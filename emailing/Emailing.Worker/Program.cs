using Emailing.Application.UseCases.ConsumeMessage;
using Emailing.Application.UseCases.SendEmail;
using Emailing.Domain.RabbitMQConnectionManager;
using Emailing.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IRabbitMQConnectionManager, RabbitMQConnectionManager>();
builder.Services.AddTransient<IConsumeMessageUseCase, ConsumeMessageUseCase>();
builder.Services.AddTransient<ISendEmailUseCase, SendEmailUseCase>();

var host = builder.Build();
host.Run();
