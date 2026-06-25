using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SitecoreAI.Dataverse.Services;

// Build host with dependency injection
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<DataverseConnectionService>();
    })
    .Build();

Console.WriteLine("Dataverse Connection Validation");

// Get the service from DI container
var dataverseService = host.Services.GetRequiredService<DataverseConnectionService>();

try
{
    // Get the service client
    var serviceClient = dataverseService.GetServiceClient();
    Console.WriteLine("Connected to Dataverse successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Dataverse call failed: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    Console.WriteLine($"\nStack Trace: {ex.StackTrace}");
}