using BackgroundTasksServices;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDBContext>();
builder.Services.AddScoped<DummyService>();

var app = builder.Build();

