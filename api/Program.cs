using api.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AddScoped<IInterface, ConcreteX>() – Creates one instance per request (Recommended for web apps).
//AddTransient<IInterface, ConcreteX>() – Creates a new instance every time it’s requested.
//AddSingleton<IInterface, ConcreteX>() – Creates a single instance for the lifetime of the application.
builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
