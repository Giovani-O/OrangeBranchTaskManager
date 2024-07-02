using Emailing.Application.UseCases.ConsumeMessage;
using Emailing.Application.UseCases.SendEmail;
using Emailing.Domain.EmailServerConfig;
using Emailing.Domain.RabbitMQConnectionManager;
using Emailing.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IRabbitMQConnectionManager, RabbitMQConnectionManager>();
builder.Services.AddTransient<ISMPTConfig, SMTPConfig>();
builder.Services.AddTransient<ISendEmailUseCase, SendEmailUseCase>();
builder.Services.AddTransient<IConsumeMessageUseCase, ConsumeMessageUseCase>();

var host = builder.Build();
host.Run();
