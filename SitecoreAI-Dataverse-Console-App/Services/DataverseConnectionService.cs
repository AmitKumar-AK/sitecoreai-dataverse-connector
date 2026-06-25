using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace SitecoreAI.Dataverse.Services;

public class DataverseConnectionService
{
    private readonly IConfiguration _configuration;
    private ServiceClient? _serviceClient;

    public DataverseConnectionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ServiceClient GetServiceClient()
    {
        if (_serviceClient != null && _serviceClient.IsReady)
        {
            return _serviceClient;
        }

        var clientId = _configuration["Dataverse:ClientId"];
        var clientSecret = _configuration["Dataverse:ClientSecret"];
        var environment = _configuration["Dataverse:DataverseEnvironment"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(environment))
        {
            throw new InvalidOperationException("Dataverse configuration is missing or incomplete.");
        }

        var connectionString = $"Url=https://{environment};AuthType=ClientSecret;ClientId={clientId};ClientSecret={clientSecret};RequireNewInstance=true";
        _serviceClient = new ServiceClient(connectionString);

        if (!_serviceClient.IsReady)
        {
            throw new InvalidOperationException($"Failed to connect to Dataverse: {_serviceClient.LastError}");
        }

        return _serviceClient;
    }
}





