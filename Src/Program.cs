using Indue.Ebt.CustomerRegistration.EntraId;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.ConfigureFunctionsWebApplication();

// Entra Id access token validation configuration
builder.Services.AddSingleton<IAccessTokenValidator>(new AccessTokenValidator 
{
    Authority = builder.Configuration.GetValue<string>("EntraId:Authority"),
    Audience = builder.Configuration.GetValue<string>("EntraId:Audience")
});

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
//builder.Services
//    .AddApplicationInsightsTelemetryWorkerService()
//    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
