using SitecoreAI.Dataverse.AzureFunctions.Models;

namespace SitecoreAI.Dataverse.AzureFunctions.Services;

/// <summary>
/// Service interface for managing enquiry records in Microsoft Dataverse.
/// Provides CRUD operations and query capabilities for enquiry data.
/// </summary>
public interface IDataverseService
{
    /// <summary>
    /// Creates a new enquiry record in Dataverse.
    /// </summary>
    /// <param name="request">The request containing enquiry details to create.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The unique identifier (GUID) of the newly created enquiry.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails or field lengths exceed maximum allowed values.</exception>
    Task<Guid> CreateEnquiryAsync(CreateEnquiryRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Updates an existing enquiry record in Dataverse.
    /// </summary>
    /// <param name="request">The request containing the enquiry ID and fields to update.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails or field lengths exceed maximum allowed values.</exception>
    Task UpdateEnquiryAsync(UpdateEnquiryRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Deletes an enquiry record from Dataverse.
    /// </summary>
    /// <param name="id">The unique identifier of the enquiry to delete.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteEnquiryAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a single enquiry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the enquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The enquiry response if found; otherwise, null.</returns>
    Task<EnquiryResponse?> GetEnquiryByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves multiple enquiries from Dataverse, ordered by creation date descending.
    /// </summary>
    /// <param name="topCount">The maximum number of enquiries to retrieve. Default is 100.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of enquiry responses.</returns>
    Task<IEnumerable<EnquiryResponse>> GetEnquiriesAsync(int topCount = 100, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all enquiries associated with a specific email address, ordered by creation date descending.
    /// </summary>
    /// <param name="email">The email address to filter enquiries by.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of enquiry responses matching the specified email.</returns>
    Task<IEnumerable<EnquiryResponse>> GetEnquiriesByEmailAsync(string email, CancellationToken cancellationToken = default);
}