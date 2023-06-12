using IPSTemplate.AppServer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((context, logger) =>
        logger.ReadFrom.Configuration(context.Configuration)
            .Enrich.WithProperty("Application", Constants.Swagger.ApiName)
            .Enrich.WithProperty("Version", Constants.Swagger.Version)
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName))
    .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables());

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("App started");

app.Run();
