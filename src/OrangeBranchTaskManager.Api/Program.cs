using Microsoft.OpenApi.Models;
using OrangeBranchTaskManager.Api.Filters;
using OrangeBranchTaskManager.Api.Middlewares;
using OrangeBranchTaskManager.Application;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
// builder.Services.AddScoped<ITokenServiceUseCase, TokenServiceUseCase>();
builder.Services.AddScoped<IRabbitMQConnectionManager, RabbitMQConnectionManager>();
// builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
// builder.Services.AddScoped<ISendEmailUseCase, SendEmailUseCase>();
// builder.Services.AddAutoMapper(typeof(TaskDTOMappingProfile));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:5173");
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
