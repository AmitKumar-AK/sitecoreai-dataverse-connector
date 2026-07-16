using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SitecoreAI.Dataverse.AzureFunctions.Models;
using SitecoreAI.Dataverse.AzureFunctions.Services;
using System.Net;

namespace SitecoreAI.Dataverse.AzureFunctions.Functions;

/// <summary>
/// Azure Function for retrieving enquiry records filtered by email address from Microsoft Dataverse.
/// </summary>
public sealed class GetEnquiriesByEmailFunction
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<GetEnquiriesByEmailFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEnquiriesByEmailFunction"/> class.
    /// </summary>
    /// <param name="dataverseService">Service for interacting with Dataverse.</param>
    /// <param name="logger">Logger for tracking function execution.</param>
    public GetEnquiriesByEmailFunction(
        IDataverseService dataverseService,
        ILogger<GetEnquiriesByEmailFunction> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    /// <summary>
    /// HTTP-triggered function that retrieves all enquiries associated with a specific email address.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="email">The email address to filter enquiries by.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>An HTTP response containing the list of enquiries for the specified email or an error message.</returns>
    [Function("GetEnquiriesByEmail")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "enquiries/email/{email}")] HttpRequestData req,
        string email,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(Constants.LogMessages.ProcessingGetEnquiriesByEmailRequest, email);

        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(
                    ApiResponse<object>.Fail(Constants.ErrorMessages.EmailParameterIsRequired),
                    cancellationToken); 
                return badResponse;
            }

            var enquiries = await _dataverseService.GetEnquiriesByEmailAsync(
                email,
                cancellationToken);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(
                ApiResponse<IEnumerable<EnquiryResponse>>.Ok(
                    enquiries,
                    string.Format(Constants.SuccessMessages.EnquiriesRetrievedForEmailFormat, enquiries.Count(), email)),
                cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Constants.LogMessages.ErrorRetrievingEnquiriesForEmail, email);

            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(
                ApiResponse<object>.Fail(string.Format(Constants.ErrorMessages.ErrorRetrievingEnquiries, ex.Message)),
                cancellationToken);
            return errorResponse;
        }
    }
}