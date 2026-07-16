using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SitecoreAI.Dataverse.AzureFunctions.Models;
using SitecoreAI.Dataverse.AzureFunctions.Services;
using System.Net;

namespace SitecoreAI.Dataverse.AzureFunctions.Functions;

/// <summary>
/// Azure Function for retrieving a single enquiry record by ID from Microsoft Dataverse.
/// </summary>
public sealed class GetEnquiryFunction
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<GetEnquiryFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEnquiryFunction"/> class.
    /// </summary>
    /// <param name="dataverseService">Service for interacting with Dataverse.</param>
    /// <param name="logger">Logger for tracking function execution.</param>
    public GetEnquiryFunction(
        IDataverseService dataverseService,
        ILogger<GetEnquiryFunction> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    /// <summary>
    /// HTTP-triggered function that retrieves a single enquiry by its unique identifier.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="id">The unique identifier of the enquiry (as a string that will be parsed to GUID).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>An HTTP response containing the enquiry or an error message.</returns>
    [Function("GetEnquiry")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "enquiries/{id}")] HttpRequestData req,
        string id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(Constants.LogMessages.ProcessingGetEnquiryRequest, id);

        try
        {
            if (!Guid.TryParse(id, out var enquiryId))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(
                    ApiResponse<object>.Fail(Constants.ErrorMessages.InvalidIdFormat),
                    cancellationToken);
                return badResponse;
            }

            var enquiry = await _dataverseService.GetEnquiryByIdAsync(
                enquiryId,
                cancellationToken);

            if (enquiry == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(
                    ApiResponse<object>.Fail(Constants.ErrorMessages.EnquiryNotFound),
                    cancellationToken);
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(
                ApiResponse<EnquiryResponse>.Ok(enquiry, Constants.SuccessMessages.EnquiryRetrievedSuccessfully),
                cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Constants.LogMessages.ErrorRetrievingEnquiryWithId, id);

            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(
                ApiResponse<object>.Fail(string.Format(Constants.ErrorMessages.ErrorRetrievingEnquiry, ex.Message)),
                cancellationToken);
            return errorResponse;
        }
    }
}