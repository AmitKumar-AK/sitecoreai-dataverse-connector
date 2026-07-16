using SitecoreAI.Dataverse.AzureFunctions.Models;
using SitecoreAI.Dataverse.AzureFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace SitecoreAI.Dataverse.AzureFunctions.Functions;

/// <summary>
/// Azure Function for deleting enquiry records from Microsoft Dataverse.
/// </summary>
public sealed class DeleteDataverseRecord
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<DeleteDataverseRecord> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteDataverseRecord"/> class.
    /// </summary>
    /// <param name="dataverseService">Service for interacting with Dataverse.</param>
    /// <param name="logger">Logger for tracking function execution.</param>
    public DeleteDataverseRecord(
        IDataverseService dataverseService,
        ILogger<DeleteDataverseRecord> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    /// <summary>
    /// HTTP-triggered function that deletes an enquiry record from Dataverse.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="id">The unique identifier of the enquiry to delete.</param>
    /// <param name="executionContext">The function execution context.</param>
    /// <returns>An HTTP response indicating success or failure of the delete operation.</returns>
    [Function("DeleteDataverseRecord")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "dataverse/enquiries/{id:guid}")]
        HttpRequestData req,
        Guid id,
        FunctionContext executionContext)
    {
        var cancellationToken = executionContext.CancellationToken;

        try
        {
            await _dataverseService.DeleteEnquiryAsync(id, cancellationToken);

            return await CreateJsonResponse(
                req,
                HttpStatusCode.OK,
                ApiResponse<object>.Ok(new { id }, "Dataverse record deleted successfully."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Dataverse record. RecordId: {RecordId}", id);

            return await CreateJsonResponse(
                req,
                HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail("An unexpected error occurred while deleting the record."));
        }
    }

    /// <summary>
    /// Creates an HTTP response with JSON content and specified status code.
    /// </summary>
    /// <typeparam name="T">The type of data in the API response.</typeparam>
    /// <param name="req">The HTTP request to create a response for.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <param name="payload">The API response payload.</param>
    /// <returns>An HTTP response with JSON content.</returns>
    private static async Task<HttpResponseData> CreateJsonResponse<T>(
        HttpRequestData req,
        HttpStatusCode statusCode,
        ApiResponse<T> payload)
    {
        var response = req.CreateResponse(statusCode);
        await response.WriteAsJsonAsync(payload);
        return response;
    }
}