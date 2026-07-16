using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SitecoreAI.Dataverse.AzureFunctions.Models;
using SitecoreAI.Dataverse.AzureFunctions.Services;
using System.Net;

namespace SitecoreAI.Dataverse.AzureFunctions.Functions;

/// <summary>
/// Azure Function for retrieving multiple enquiry records from Microsoft Dataverse.
/// </summary>
public sealed class GetEnquiriesFunction
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<GetEnquiriesFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEnquiriesFunction"/> class.
    /// </summary>
    /// <param name="dataverseService">Service for interacting with Dataverse.</param>
    /// <param name="logger">Logger for tracking function execution.</param>
    public GetEnquiriesFunction(
        IDataverseService dataverseService,
        ILogger<GetEnquiriesFunction> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    /// <summary>
    /// HTTP-triggered function that retrieves enquiries from Dataverse.
    /// Supports optional 'top' query parameter to limit the number of results (default: 100, max: 1000).
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>An HTTP response containing the list of enquiries or an error message.</returns>
    [Function("GetEnquiries")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "enquiries")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(Constants.LogMessages.ProcessingGetEnquiriesRequest);

        try
        {
            var topCount = 100;
            if (req.Query["top"] != null && int.TryParse(req.Query["top"], out var parsedTop))
            {
                topCount = parsedTop > 0 && parsedTop <= 1000 ? parsedTop : 100;
            }

            var enquiries = await _dataverseService.GetEnquiriesAsync(
                topCount,
                cancellationToken);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(
                ApiResponse<IEnumerable<EnquiryResponse>>.Ok(
                    enquiries,
                    string.Format(Constants.SuccessMessages.EnquiriesRetrievedFormat, enquiries.Count())),
                cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Constants.LogMessages.ErrorRetrievingEnquiries);

            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(
                ApiResponse<object>.Fail(string.Format(Constants.ErrorMessages.ErrorRetrievingEnquiries, ex.Message)),
                cancellationToken);
            return errorResponse;
        }
    }
}