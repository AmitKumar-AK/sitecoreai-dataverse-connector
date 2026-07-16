using SitecoreAI.Dataverse.AzureFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using SitecoreAI.Dataverse.AzureFunctions.Models;

namespace SitecoreAI.Dataverse.AzureFunctions.Functions;

/// <summary>
/// Azure Function for creating new enquiry records in Microsoft Dataverse.
/// </summary>
public sealed class CreateDataverseRecord
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<CreateDataverseRecord> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDataverseRecord"/> class.
    /// </summary>
    /// <param name="dataverseService">Service for interacting with Dataverse.</param>
    /// <param name="logger">Logger for tracking function execution.</param>
    public CreateDataverseRecord(
        IDataverseService dataverseService,
        ILogger<CreateDataverseRecord> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    /// <summary>
    /// HTTP-triggered function that creates a new enquiry record in Dataverse.
    /// </summary>
    /// <param name="req">The HTTP request containing the enquiry data in the body.</param>
    /// <param name="executionContext">The function execution context.</param>
    /// <returns>An HTTP response with the created record ID or an error message.</returns>
    [Function("CreateDataverseRecord")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "dataverse/enquiries")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var cancellationToken = executionContext.CancellationToken;

        try
        {
            var request = await JsonSerializer.DeserializeAsync<CreateEnquiryRequest>(
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

            var validationError = ValidateCreateRequest(request);
            if (validationError is not null)
            {
                return await CreateJsonResponse(
                    req,
                    HttpStatusCode.BadRequest,
                    ApiResponse<object>.Fail(validationError));
            }

            var id = await _dataverseService.CreateEnquiryAsync(request, cancellationToken);

            return await CreateJsonResponse(
                req,
                HttpStatusCode.Created,
                ApiResponse<object>.Ok(new { id }, Constants.SuccessMessages.EnquiryCreatedSuccessfully));
        }
        catch (ArgumentException argEx)
        {
            // Validation error from service
            _logger.LogWarning(argEx, "Validation error while creating Dataverse record");

            return await CreateJsonResponse(
                req,
                HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(argEx.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Constants.LogMessages.FailedToCreateDataverseRecord);

            return await CreateJsonResponse(
                req,
                HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail(Constants.ErrorMessages.UnexpectedErrorCreating));
        }
    }

    /// <summary>
    /// Validates the create enquiry request for required fields.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>An error message if validation fails; otherwise, null.</returns>
    private static string? ValidateCreateRequest(CreateEnquiryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Constants.ErrorMessages.NameIsRequired;
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Constants.ErrorMessages.EmailIsRequired;
        }

        return null;
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
