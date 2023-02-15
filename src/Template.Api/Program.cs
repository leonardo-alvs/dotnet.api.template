using Microsoft.AspNetCore.Mvc.ApiExplorer;

using Serilog;
using Template.Api.Config;
using Template.Api.Filters;
using Template.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
SerilogConfig.AddSerilogConfig();
builder.Host.UseSerilog();

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.Sources.Clear();
    var env = hostingContext.HostingEnvironment;

    config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

    if (args != null)
    {
        config.AddCommandLine(args);
    }
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddVersioningConfig();

builder.Services.AddControllers(opt => opt.Filters.Add<NotificationFilter>()).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSwaggerConfig();
builder.Services.AddEndpointsApiExplorer();

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(5001);
});

builder.Services.AddDatabaseConfiguration(builder.Configuration);
//builder.Services.AddAuthenticationConfig();
builder.Services.AddDependencyInjection();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

//Add Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.Services.AddHealthCheckConfig();

var app = builder.Build();

app.AddHealthCheckMap();

app.UseCors();
//app.UseAuthentication();
//app.UseAuthorization();
//middleware que injeta informações da request no log
app.UseMiddleware<SerilogHttpRequestMiddleware>();
app.UseSerilogRequestLogging();
//middleware que trata as exceções e escreve no log
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsEnvironment("prd"))
{
    app.UseSwaggerConfig(app.Services
        .GetRequiredService<IApiVersionDescriptionProvider>());
}

app.Run();
