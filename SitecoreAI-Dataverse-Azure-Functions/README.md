# SitecoreAI Dataverse Azure Functions

Azure Functions application for managing enquiry records in Microsoft Dataverse. This project provides a RESTful API for CRUD operations on enquiry data, integrated with SitecoreAI applications.

## Features

- ✅ Create, Read, Update, and Delete (CRUD) enquiry records
- ✅ Query enquiries by ID or email address
- ✅ Automatic field validation and length constraints
- ✅ Comprehensive error handling and logging
- ✅ Integration with Microsoft Dataverse using OAuth client credentials
- ✅ Structured JSON responses with success/error messages

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Azure Functions Core Tools v4](https://docs.microsoft.com/azure/azure-functions/functions-run-local)
- Microsoft Dataverse instance
- Azure App Registration with client credentials (ClientId and ClientSecret)

## Configuration

### Local Development

Create or update `local.settings.json` in the project root:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ClientId": "your-azure-app-client-id",
    "ClientSecret": "your-azure-app-client-secret",
    "DataverseEnvironment": "your-environment.crm.dynamics.com"
  }
}
```

### Azure Deployment

Configure the following Application Settings in your Azure Function App:

| Setting | Description |
|---------|-------------|
| `ClientId` | Azure AD App Registration Client ID |
| `ClientSecret` | Azure AD App Registration Client Secret |
| `DataverseEnvironment` | Dataverse environment URL (e.g., `yourorg.crm.dynamics.com`) |

## API Endpoints

### Create Enquiry
**POST** `/api/dataverse/enquiries`

Creates a new enquiry record.

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "message": "I would like more information about your services.",
  "source": "Website"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Dataverse record created successfully.",
  "data": {
    "id": "12345678-1234-1234-1234-123456789abc"
  }
}
```

---

### Update Enquiry
**PATCH** `/api/dataverse/enquiries/{id}`

Updates an existing enquiry record. Only provided fields will be updated.

**Request Body:**
```json
{
  "name": "Jane Doe",
  "email": "jane.doe@example.com"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Dataverse record updated successfully.",
  "data": {
    "id": "12345678-1234-1234-1234-123456789abc"
  }
}
```

---

### Delete Enquiry
**DELETE** `/api/dataverse/enquiries/{id}`

Deletes an enquiry record.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Dataverse record deleted successfully.",
  "data": {
    "id": "12345678-1234-1234-1234-123456789abc"
  }
}
```

---

### Get Enquiry by ID
**GET** `/api/enquiries/{id}`

Retrieves a single enquiry by its unique identifier.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Enquiry retrieved successfully",
  "data": {
    "id": "12345678-1234-1234-1234-123456789abc",
    "name": "John Doe",
    "email": "john.doe@example.com",
    "message": "I would like more information about your services.",
    "source": "Website",
    "createdOn": "2026-07-17T10:30:00Z",
    "modifiedOn": "2026-07-17T10:30:00Z"
  }
}
```

---

### Get All Enquiries
**GET** `/api/enquiries?top=100`

Retrieves all enquiries, ordered by creation date (descending).

**Query Parameters:**
- `top` (optional): Maximum number of records to return (default: 100, max: 1000)

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Retrieved 25 enquiries",
  "data": [
    {
      "id": "12345678-1234-1234-1234-123456789abc",
      "name": "John Doe",
      "email": "john.doe@example.com",
      "message": "Sample message",
      "source": "Website",
      "createdOn": "2026-07-17T10:30:00Z",
      "modifiedOn": "2026-07-17T10:30:00Z"
    }
  ]
}
```

---

### Get Enquiries by Email
**GET** `/api/enquiries/email/{email}`

Retrieves all enquiries for a specific email address.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Retrieved 3 enquiries for john.doe@example.com",
  "data": [
    {
      "id": "12345678-1234-1234-1234-123456789abc",
      "name": "John Doe",
      "email": "john.doe@example.com",
      "message": "First enquiry",
      "source": "Website",
      "createdOn": "2026-07-17T10:30:00Z",
      "modifiedOn": "2026-07-17T10:30:00Z"
    }
  ]
}
```

## Field Constraints

All text fields have a maximum length of **100 characters**:
- Name
- Email
- Message
- Source

Values exceeding these limits will be automatically truncated, and validation errors will be returned if appropriate.

## Local Development

### Running Locally

1. Clone the repository
2. Navigate to the Functions project directory:
   ```bash
   cd SitecoreAI-Dataverse-Azure-Functions
   ```

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Configure `local.settings.json` with your Dataverse credentials

5. Start the Functions host:
   ```bash
   func start
   ```

6. The API will be available at `http://localhost:7071`

### Testing with cURL

**Create an enquiry:**
```bash
curl -X POST http://localhost:7071/api/dataverse/enquiries \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "test@example.com",
    "message": "This is a test enquiry",
    "source": "API Test"
  }'
```

**Get all enquiries:**
```bash
curl http://localhost:7071/api/enquiries
```

## Project Structure

```
SitecoreAI-Dataverse-Azure-Functions/
├── Functions/                      # Azure Function endpoints
│   ├── CreateDataverseRecord.cs    # POST /api/dataverse/enquiries
│   ├── UpdateDataverseRecord.cs    # PATCH /api/dataverse/enquiries/{id}
│   ├── DeleteDataverseRecord.cs    # DELETE /api/dataverse/enquiries/{id}
│   ├── GetEnquiryFunction.cs       # GET /api/enquiries/{id}
│   ├── GetEnquiriesFunction.cs     # GET /api/enquiries
│   └── GetEnquiriesByEmailFunction.cs # GET /api/enquiries/email/{email}
├── Services/                       # Business logic and data access
│   ├── IDataverseService.cs        # Service interface
│   └── DataverseService.cs         # Dataverse integration implementation
├── Models/                         # Data models
│   └── CreateEnquiryRequest.cs     # Request/Response models
├── Constants.cs                    # Application constants and messages
├── Program.cs                      # Application entry point and DI configuration
├── host.json                       # Function host configuration
└── local.settings.json            # Local development settings (not in source control)
```

## Key Dependencies

- **Microsoft.Azure.Functions.Worker** - Azure Functions runtime for isolated worker process
- **Microsoft.PowerPlatform.Dataverse.Client** - Dataverse SDK for authentication and data operations
- **Microsoft.Xrm.Sdk** - Core SDK for Dynamics 365/Dataverse

## Error Handling

The API returns consistent error responses:

**400 Bad Request** - Validation errors, missing required fields, or invalid data
```json
{
  "success": false,
  "message": "Name is required."
}
```

**404 Not Found** - Enquiry record not found
```json
{
  "success": false,
  "message": "Enquiry not found"
}
```

**500 Internal Server Error** - Unexpected errors
```json
{
  "success": false,
  "message": "An unexpected error occurred while creating the record."
}
```

## Deployment to Azure

### Using Azure CLI

1. Build the project:
   ```bash
   dotnet build --configuration Release
   ```

2. Publish to Azure:
   ```bash
   func azure functionapp publish <your-function-app-name>
   ```

### Using Visual Studio

1. Right-click the project in Solution Explorer
2. Select **Publish**
3. Choose **Azure** as the target
4. Select your Function App or create a new one
5. Click **Publish**

### Post-Deployment

Don't forget to configure Application Settings in the Azure Portal with your `ClientId`, `ClientSecret`, and `DataverseEnvironment`.

## Dataverse Configuration

### Required Permissions

Your Azure AD App Registration needs the following permissions:
- **Dynamics CRM API** - `user_impersonation` (Delegated) or appropriate application permissions

### Table Schema

The functions interact with the `crf51_sitecoreenquiry` table with the following columns:
- `crf51_newcolumn` (Name)
- `crf51_email` (Email)
- `crf51_message` (Message)
- `crf51_source` (Source)
- `createdon` (Created On)
- `modifiedon` (Modified On)

## Logging

The application uses structured logging with Microsoft.Extensions.Logging:
- Information logs for successful operations
- Warning logs for validation failures and not found scenarios
- Error logs for unexpected exceptions

Logs are available in:
- **Local Development**: Console output
- **Azure**: Application Insights (if configured)

## Support

For issues or questions related to this project, please contact the development team or create an issue in the repository.

## License

See the [LICENSE](../LICENSE) file in the repository root for licensing information.
