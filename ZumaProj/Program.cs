using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Zuma.Application.Interfaces;
using Zuma.Domain.Interfaces.IRepositories;
using Zuma.Infrastructure.Context;
using Zuma.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddTransient<IBotUserRepository, BotUserRepository>();
var botToken = builder.Configuration.GetSection("TelegramBot")["Token"];
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddSingleton<IUserSessionService, Zuma.Infrastructure.Services.UserSessionService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
