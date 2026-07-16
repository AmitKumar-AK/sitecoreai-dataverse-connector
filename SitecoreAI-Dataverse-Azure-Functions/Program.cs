using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SitecoreAI.Dataverse.AzureFunctions.Services;

// Create and configure the Azure Functions application builder
var builder = FunctionsApplication.CreateBuilder(args);

// Configure the Functions Web Application middleware
builder.ConfigureFunctionsWebApplication();

// Register application services for dependency injection
// IDataverseService: Service for interacting with Microsoft Dataverse
builder.Services.AddSingleton<IDataverseService, DataverseService>();

// Build and run the application
builder.Build().Run();