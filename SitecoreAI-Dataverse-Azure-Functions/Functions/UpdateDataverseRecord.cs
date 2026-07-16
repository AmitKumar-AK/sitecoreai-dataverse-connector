using SitecoreAI.Dataverse.AzureFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using SitecoreAI.Dataverse.AzureFunctions.Models;

namespace SitecoreAI.Dataverse.AzureFunctions.Functions;

/// <summary>
/// Azure Function for updating existing enquiry records in Microsoft Dataverse.
/// </summary>
public sealed class UpdateDataverseRecord
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<UpdateDataverseRecord> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDataverseRecord"/> class.
    /// </summary>
    /// <param name="dataverseService">Service for interacting with Dataverse.</param>
    /// <param name="logger">Logger for tracking function execution.</param>
    public UpdateDataverseRecord(
        IDataverseService dataverseService,
        ILogger<UpdateDataverseRecord> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    /// <summary>
    /// HTTP-triggered function that updates an existing enquiry record in Dataverse.
    /// </summary>
    /// <param name="req">The HTTP request containing the updated enquiry data in the body.</param>
    /// <param name="id">The unique identifier of the enquiry to update.</param>
    /// <param name="executionContext">The function execution context.</param>
    /// <returns>An HTTP response indicating success or failure of the update operation.</returns>
    [Function("UpdateDataverseRecord")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "dataverse/enquiries/{id:guid}")]
        HttpRequestData req,
        Guid id,
        FunctionContext executionContext)
    {
        var cancellationToken = executionContext.CancellationToken;

        try
        {
            var request = await JsonSerializer.DeserializeAsync<UpdateEnquiryRequest>(
                req.Body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                },
                cancellationToken);

            if (request is null)
            {
                return await CreateJsonResponse(
                    req,
                    HttpStatusCode.BadRequest,
                    ApiResponse<object>.Fail(Constants.ErrorMessages.RequestBodyIsRequired));
            }

            request.Id = id;

            await _dataverseService.UpdateEnquiryAsync(request, cancellationToken);

            return await CreateJsonResponse(
                req,
                HttpStatusCode.OK,
                ApiResponse<object>.Ok(new { id }, Constants.SuccessMessages.EnquiryUpdatedSuccessfully));
        }
        catch (ArgumentException argEx)
        {
            // Validation error from service
            _logger.LogWarning(argEx, "Validation error while updating Dataverse record: {RecordId}", id);

            return await CreateJsonResponse(
                req,
                HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(argEx.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Constants.LogMessages.FailedToUpdateDataverseRecord, id);

            return await CreateJsonResponse(
                req,
                HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail(Constants.ErrorMessages.UnexpectedErrorUpdating));
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