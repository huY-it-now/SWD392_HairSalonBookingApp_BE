using Infrastructures;
using WebAPI;
using Application.Commons;
using Domain.Contracts.Settings;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);

// parse the configuration in appsettings
var configuration = builder.Configuration.Get<AppConfiguration>();

IConfiguration configurationPayOs = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

PayOS payOS = new PayOS(configurationPayOs["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                    configurationPayOs["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                    configurationPayOs["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));

builder.Services.AddSingleton(payOS);

builder.Services.AddInfrastructuresService(configuration.DatabaseConnection);
builder.Services.AddWebAPIService();
builder.Services.AddSingleton(configuration);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Maverick Deploy",
        Version = "v1"
    });
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("JWTSecretKey"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

/*
    register with singleton life time
    now we can use dependency injection for AppConfiguration
*/
builder.Services.AddSingleton(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseDeveloperExceptionPage();

app.MapHealthChecks("/healthchecks");
app.UseHttpsRedirection();
app.UseCors();
// todo authentication
app.UseAuthorization();

app.MapControllers();

app.Run();

// this line tell intergrasion test
// https://stackoverflow.com/questions/69991983/deps-file-missing-for-dotnet-6-integration-tests
public partial class Program { }