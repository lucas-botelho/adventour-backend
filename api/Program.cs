using Adventour.Api.Configurations;
using Adventour.Api.Data;
using Adventour.Api.Repositories;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Services.Authentication;
using Adventour.Api.Services.Database;
using Adventour.Api.Services.DistanceCalculation.Interfaces;
using Adventour.Api.Services.DistanceCalculation;
using Adventour.Api.Services.Email.Interfaces;
using Adventour.Api.Services.FileUpload;
using Adventour.Api.Services.FileUpload.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

//EF database connection
builder.Services.AddDbContext<AdventourContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING")));


//Transient objects are always different; a new instance is provided to every controller and every service.
//builder.Services.AddTransient

//Scoped objects are the same within a request, but different across different requests.
//builder.Services.AddScoped
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAttractionRepository, AttractionRepository>();
builder.Services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
builder.Services.AddScoped<IDayRepository, DayRepository>();
builder.Services.AddScoped<IItineraryRepository, ItineraryRepository>();
//builder.Services.AddScoped<IQueryServiceBuilder, QueryServiceBuilder>();
//builder.Services.AddScoped<IDatabaseService, MsSqlService>();
builder.Services.AddScoped<IFileUploadService, CloudinaryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IEmailService, SendGridService>();

builder.Services.AddHttpClient<IGeoLocationService, TomTomService>();

builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));

//Singleton objects are the same for every object and every request.
builder.Services.AddSingleton<ITokenProviderService, JwtTokenProviderService>();


builder.Services.AddHttpClient();


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

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson($@"
    {{
        ""type"": ""service_account"",
        ""project_id"": ""{Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")}"",
        ""private_key_id"": ""{Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID")}"",
        ""private_key"": ""{Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY").Replace("\\n", "\n")}"",
        ""client_email"": ""{Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL")}"",
        ""client_id"": ""{Environment.GetEnvironmentVariable("FIREBASE_CLIENT_ID")}"",
        ""auth_uri"": ""{Environment.GetEnvironmentVariable("FIREBASE_AUTH_URI")}"",
        ""token_uri"": ""{Environment.GetEnvironmentVariable("FIREBASE_TOKEN_URI")}"",
        ""auth_provider_x509_cert_url"": ""{Environment.GetEnvironmentVariable("FIREBASE_AUTH_PROVIDER_CERT_URL")}"",
        ""client_x509_cert_url"": ""{Environment.GetEnvironmentVariable("FIREBASE_CLIENT_CERT_URL")}"",
        ""universe_domain"": ""{Environment.GetEnvironmentVariable("FIREBASE_UNIVERSE_DOMAIN")}""
    }}")
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});




var app = builder.Build();

// Set the base path
var basePath = app.Configuration["BasePath"];
if (!string.IsNullOrEmpty(basePath))
{
    app.UsePathBase(basePath);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
