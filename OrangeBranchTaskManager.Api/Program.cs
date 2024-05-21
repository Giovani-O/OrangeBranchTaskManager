using Microsoft.EntityFrameworkCore;
using OrangeBranchTaskManager.Api.Controllers.Mappings;
using OrangeBranchTaskManager.Api.Data;
using OrangeBranchTaskManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conection String pode mudar dependendo do seu ambiente, altere em appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Define a conexão
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ITasksService, TasksService>();

builder.Services.AddAutoMapper(typeof(TaskDTOMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
