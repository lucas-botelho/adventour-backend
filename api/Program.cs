
using Adventour.Api.Builders;
using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Configurations;
using Adventour.Api.Repositories;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Services.Authentication;
using Adventour.Api.Services.Database;
using Adventour.Api.Services.Email.Interfaces;
using Adventour.Api.Services.FileUpload;
using Adventour.Api.Services.FileUpload.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Transient objects are always different; a new instance is provided to every controller and every service.
//builder.Services.AddTransient

//Scoped objects are the same within a request, but different across different requests.
//builder.Services.AddScoped
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQueryServiceBuilder, QueryServiceBuilder>();
builder.Services.AddScoped<IDatabaseConnectionService, DbConnectionService>();
builder.Services.AddScoped<IFileUploadService, CloudinaryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IEmailService, SendGridService>();
builder.Services.AddScoped<IItineraryRepository, ItineraryRepository>();


builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));

//Singleton objects are the same for every object and every request.
builder.Services.AddSingleton<ITokenProviderService, JwtTokenProviderService>();

//Dapper settings
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = Environment.GetEnvironmentVariable("JWT_ISSUER");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            ValidateLifetime = true
        };
    });

// Add services to the container for Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter your bearer token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    //options.ExampleFilters();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
