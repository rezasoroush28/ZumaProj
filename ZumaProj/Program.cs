using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

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
