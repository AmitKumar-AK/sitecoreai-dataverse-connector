# SitecoreAI-Dataverse-Connector

> **Enterprise-grade integration connecting SitecoreAI (Sitecore XM Cloud) Forms to Microsoft Dataverse with Client-Id and Client-Secret based authentication, Azure Functions serverless middleware, and production-ready CRUD operations.**

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![Azure Functions](https://img.shields.io/badge/Azure-Functions-0078D4)](https://azure.microsoft.com/en-us/services/functions/)
[![Microsoft Dataverse](https://img.shields.io/badge/Microsoft-Dataverse-742774)](https://powerplatform.microsoft.com/en-us/dataverse/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## 📋 Table of Contents

- [Overview](#-overview)
- [What Problem Does This Solve?](#-what-problem-does-this-solve)
- [Key Features](#-key-features)
- [Architecture](#️-architecture)
- [Prerequisites](#-prerequisites)
- [Quick Start](#-quick-start)
- [Authentication Setup](#-authentication-setup)
- [Configuration](#️-configuration)
- [Usage Examples](#-usage-examples)
- [Deployment](#-deployment)
- [Troubleshooting](#-troubleshooting)
- [FAQ](#-faq)
- [Contributing](#-contributing)
- [Resources](#-resources)
- [License](#-license)

---

## 🎯 Overview

**SitecoreAI-Dataverse-Connector** is a production-ready integration solution that bridges **SitecoreAI (Sitecore XM Cloud)** headless CMS with **Microsoft Dataverse** (Microsoft's unified data platform for Power Platform, Dynamics 365, and custom business applications).

### What is This Integration?

This repository provides:
- ✅ **Azure Functions** serverless API for handling SitecoreAI form submissions
- ✅ **Client-Id and Client-Secret based authentication** for secure Dataverse connections
- ✅ **CRUD operations** (Create, Read, Update, Delete) on Dataverse entities
- ✅ **Console application** for testing and demonstration
- ✅ **Production-ready code** following Microsoft best practices

---

## 💡 What Problem Does This Solve?

### The Challenge

Organizations using **SitecoreAI (Sitecore XM Cloud)** for content management and **Microsoft Dataverse** for business data face integration challenges:

1. **Form Data Persistence** - SitecoreAI Forms need to store submissions in a centralized database
2. **Business Process Automation** - Form data must trigger workflows in Dynamics 365 or Power Automate
3. **Authentication Complexity** - Secure server-to-server authentication without exposing credentials


### The Solution

This connector provides:
- 🔐 **Client-Id and Client-Secret based authentication** by utilizing Application User requirements
- ⚡ **Serverless Azure Functions** for scalable, cost-effective integration
- 📊 **Data flow** between SitecoreAI and Dataverse
- 🛡️ **Production-ready security** with Azure Key Vault support

---

## ✨ Key Features

### Authentication & Security
- ✅ **Client-Id and Client-Secret based authentication** using Azure AD App Registration
- ✅ **Azure Key Vault integration** for secret storage (production)
- ✅ **Application User support** for Dataverse security roles

### Integration Capabilities
- ✅ **SitecoreAI Form submission handling** via webhooks
- ✅ **CRUD operations** on any Dataverse table/entity
- ✅ **Custom entity support** with strongly-typed models
- ✅ **Error handling and retry logic** for production reliability

### Developer Experience
- ✅ **Console app** for local testing and exploration
- ✅ **Environment-based configuration** using appsettings.json
- ✅ **Detailed troubleshooting guides** for common issues

### Platform Support
- ✅ **Dataverse production environments**
- ✅ **Azure Functions Consumption Plan** (serverless)
- ✅ **Azure Functions Premium/Dedicated** (enterprise)

---

## 🏗️ Architecture

```
┌─────────────────────┐
│  SitecoreAI         │  (Headless CMS)
│   (Sitecore XMC)    │
│                     │
│  ┌───────────────┐  │
│  │  Form Submit  │  │
│  └───────┬───────┘  │
└──────────┼──────────┘
           │ HTTP POST (Webhook)
           ▼
┌─────────────────────────────────────────┐
│        Azure Functions                  │
│  ┌────────────────────────────────────┐ │
│  │  SitecoreAI-Dataverse-Connector    │ │
│  │                                    │ │
│  │  • Validate request                │ │
│  │  • Transform data                  │ │
│  │  • Client-Secret auth              │ │
│  │  • CRUD operations                 │ │
│  └─────────────┬──────────────────────┘ │
└────────────────┼────────────────────────┘
                 │ ServiceClient (Secret Auth)
                 ▼
┌─────────────────────────────────────────┐
│     Microsoft Dataverse                 │
│  ┌────────────────────────────────────┐ │
│  │  Tables/Entities                   │ │
│  │  • Contact Form Submissions        │ │
│  │  • Custom Business Entities        │ │
│  │  • Dynamics 365 Data               │ │
│  └────────────────────────────────────┘ │
│                                         │
│  Power Platform Integration:            │
│  • Power Automate Flows                 │
│  • Power BI Reports                     │
│  • Dynamics 365 Apps                    │
└─────────────────────────────────────────┘
```

### Authentication Flow (Client-Id & Client-Secret)

```
┌──────────────────┐
│ Azure Functions  │
│  (Your Code)     │
└────────┬─────────┘
         │ 1. Load Client-Id & Client-Secret
         │    from environment variables or
         │    Azure Key Vault
         ▼
┌──────────────────┐
│   Azure AD       │
│ App Registration │
└────────┬─────────┘
         │ 2. Request OAuth token
         │    using Client-Id and Client-Secret
         ▼
┌──────────────────┐
│   Azure AD       │
│   OAuth Token    │
│   (JWT Bearer)   │
└────────┬─────────┘
         │ 3. Token returned
         │    (valid for ~60 minutes)
         ▼
┌──────────────────┐
│   Dataverse API  │
│  (Authenticated) │
│                  │
│ CRUD Operations  │
│ • Create         │
│ • Read           │
│ • Update         │
│ • Delete         │
└──────────────────┘
```

---

## 📦 Prerequisites

### Required Software
- ✅ **.NET 8.0 SDK** or later - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ **Visual Studio 2022** or **Visual Studio Code**
- ✅ **Azure Functions Core Tools** - [Install Guide](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- ✅ **PowerShell 5.1+** (Windows) or **PowerShell Core 7+** (cross-platform)

### Azure Resources
- ✅ **Azure Subscription** (free tier works)
- ✅ **Azure AD Tenant** access for app registration
- ✅ **Microsoft Dataverse environment**
  - MSFD Developer/Learning environment (free) - [Sign up](https://developer.microsoft.com/en-us/microsoft-365/dev-program)
  - OR Dataverse production environment

### Permissions Required
- ✅ **Azure AD**: Ability to create App Registrations
- ✅ **Dataverse**: System Administrator or equivalent role

---

## 🚀 Quick Start

### 1. Clone Repository

```bash
git clone https://github.com/yourusername/sitecoreai-dataverse-connector.git
cd sitecoreai-dataverse-connector
```

### 2. Create Azure AD App Registration

1. Navigate to [Azure Portal](https://portal.azure.com)
2. Go to **Azure Active Directory** → **App Registrations**
3. Click **New registration**:
   - Name: **"SitecoreAI-Dataverse-Connector"**
   - Supported account types: **Accounts in this organizational directory only**
   - Click **Register**
4. Copy the **Application (client) ID** and **Directory (tenant) ID**
5. Under **Certificates & secrets** → **Client secrets**:
   - Click **New client secret**
   - Description: "Dataverse Connector Secret"
   - Expires: 24 months (or per your policy)
   - Click **Add**
   - **⚠️ IMPORTANT**: Copy the secret **Value** immediately (shown only once!)
6. Under **API permissions**:
   - Click **Add a permission** → **Dynamics CRM**
   - Select **Delegated permissions** → **user_impersonation**
   - Click **Add permissions**
   - Click **Grant admin consent** (requires admin role)

### 3. Configure Environment Variables

Copy `.env.example` to `.env` and fill in your values:

```bash
AZURE_CLIENT_ID=12345678-1234-1234-1234-123456789abc
AZURE_TENANT_ID=87654321-4321-4321-4321-cba987654321
AZURE_CLIENT_SECRET=your-secret-value-from-step-2
DATAVERSE_URL=https://yourorg.crm.dynamics.com
```

⚠️ **Security Note**: Never commit `.env` to source control! It's already in `.gitignore`.

### 4. Create Application User in Dataverse

1. Navigate to [Power Platform Admin Center](https://admin.powerplatform.microsoft.com/)
2. Select your environment → **Settings** → **Users + permissions** → **Application users**
3. Click **New app user**
4. Click **Add an app** → Select your **SitecoreAI-Dataverse-Connector** app
5. Select **Business unit**
6. Click **Create**
7. Click **Edit security roles** → Assign **System Administrator** (or custom role)
8. Click **Save**

### 5. Test Connection

```bash
cd SitecoreAI-Dataverse-Console-App
dotnet run

# Expected output:
# Dataverse Connection Validation
# Connected to Dataverse successfully.
```

### 6. Verify Azure Functions (Optional)

```bash
cd SitecoreAI-Dataverse-Functions
func start

# Test the endpoint:
# POST http://localhost:7071/api/HandleFormSubmission
```

---

## 🔐 Authentication Setup

### Client-Id and Client-Secret Authentication

**Best for**: Standard Dataverse environments, production with Azure Key Vault

#### Why Client-Id and Client-Secret?

- ✅ **Simple setup** - easier to configure than certificates
- ✅ **Works with Application Users** in standard Dataverse environments
- ✅ **Azure Key Vault integration** for production security
- ✅ **Standard OAuth 2.0 flow** - widely supported

#### Setup Steps

See [Quick Start](#quick-start) above for complete walkthrough.

#### Connection String Format

```csharp
// Client Secret authentication
string connectionString = $@"
    AuthType=ClientSecret;
    Url={dataverseUrl};
    ClientId={clientId};
    ClientSecret={clientSecret};
";
```

#### Production Security Best Practices

1. **Use Azure Key Vault** to store Client Secret
2. **Rotate secrets regularly** (every 90-180 days)
3. **Never commit secrets** to source control
4. **Use Managed Identity** when possible (Azure Functions)
5. **Monitor secret expiration** dates

⚠️ **Security Warning**: Client Secrets should never be committed to source control. Always use environment variables or Azure Key Vault in production.

---

## ⚙️ Configuration

### Environment Variables

Create a `.env` file in the project root (copy from `.env.example`):

```bash
## Azure AD / App Registration
AZURE_CLIENT_ID=12345678-1234-1234-1234-123456789abc
AZURE_TENANT_ID=87654321-4321-4321-4321-cba987654321
AZURE_CLIENT_SECRET=your-secret-here  # Only if not using certificate

## Dataverse Connection
DATAVERSE_URL=https://orgb25b7c5c.crm.dynamics.com
DATAVERSE_API_VERSION=9.2

## Logging
LOG_LEVEL=Information
```

### appsettings.json (Console App)

```json
{
  "Dataverse": {
    "Url": "https://yourorg.crm.dynamics.com",
    "ClientId": "your-client-id",
    "TenantId": "your-tenant-id",
    "ClientSecret": "your-client-secret"
  }
}
```

### Connection String Format

```csharp
// Client Secret authentication (Standard)
string connectionString = $@"
    AuthType=ClientSecret;
    Url={dataverseUrl};
    ClientId={clientId};
    ClientSecret={clientSecret};
";
```

---

## 💻 Usage Examples

### Example 1: Create Dataverse Record

```csharp
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

// Connect to Dataverse
var serviceClient = new ServiceClient(connectionString);

// Create new contact form submission
var entity = new Entity("cr_contactformsubmission");
entity["cr_name"] = "John Doe";
entity["cr_email"] = "john.doe@example.com";
entity["cr_message"] = "Interested in your services";
entity["cr_submitteddate"] = DateTime.UtcNow;

// Save to Dataverse
Guid recordId = serviceClient.Create(entity);
Console.WriteLine($"✅ Created record with ID: {recordId}");
```

### Example 2: Query Records

```csharp
// Query using QueryExpression
var query = new QueryExpression("crf51_department")
{
    ColumnSet = new ColumnSet("crf51_name", "crf51_description"),
    Criteria = new FilterExpression
    {
        Conditions =
        {
            new ConditionExpression("statecode", ConditionOperator.Equal, 0)
        }
    }
};

EntityCollection results = serviceClient.RetrieveMultiple(query);

foreach (var dept in results.Entities)
{
    Console.WriteLine($"Department: {dept.GetAttributeValue<string>("crf51_name")}");
}
```

### Example 3: Azure Function Handler

```csharp
[Function("HandleFormSubmission")]
public async Task<HttpResponseData> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
{
    // Parse Sitecore form data
    var formData = await req.ReadFromJsonAsync<SitecoreFormSubmission>();
    
    // Create Dataverse entity
    var entity = new Entity("cr_contactformsubmission");
    entity["cr_name"] = formData.Name;
    entity["cr_email"] = formData.Email;
    entity["cr_message"] = formData.Message;
    
    // Save to Dataverse
    var serviceClient = new ServiceClient(Environment.GetEnvironmentVariable("DATAVERSE_CONNECTION"));
    Guid recordId = serviceClient.Create(entity);
    
    // Return success response
    var response = req.CreateResponse(HttpStatusCode.OK);
    await response.WriteAsJsonAsync(new { id = recordId });
    return response;
}
```
---

## 🚀 Deployment

### Deploy to Azure Functions

#### Visual Studio Publish

1. Right-click Azure Functions project → **Publish**
2. Select **Azure** → **Azure Function App (Windows)**
3. Choose existing or create new Function App
4. Click **Publish**

---

## 🔧 Troubleshooting

### Common Issues & Solutions

#### Issue 1: Client Secret Expired

**Error**: `AADSTS7000222: The provided client secret keys are expired`

**Solution**:
```bash
# In Azure Portal:
# 1. Go to App Registrations → Your App → Certificates & secrets
# 2. Delete expired secret
# 3. Create new client secret
# 4. Update .env file with new secret value
# 5. Restart application
```

#### Issue 2: Invalid Client Secret

**Error**: `AADSTS7000215: Invalid client secret provided`

**Solution**:
1. Verify you copied the secret **Value** (not Secret ID)
2. Check for extra spaces or newlines in `.env` file
3. Ensure secret hasn't expired
4. Regenerate secret if necessary

#### Issue 2: Field Name Case Sensitivity

**Error**: `'crf51_Department' entity doesn't contain attribute with Name = 'crf51_ID'`

**Solution**: Dataverse field names are **always lowercase** in code:

```csharp
// ❌ WRONG
entity["crf51_ID"] = 123;

// ✅ CORRECT
entity["crf51_id"] = 123;
```

#### Issue 3: Application User Missing Permissions

**Error**: `Principal user (Id=...) is missing prvCreate privilege`

**Solution**:
1. Navigate to Power Platform Admin Center
2. Go to your environment → Settings → Users + permissions → Application users
3. Select your app user
4. Click **Edit security roles**
5. Assign **System Administrator** or custom role with required privileges
6. Click **Save**

#### Issue 4: API Permissions Not Granted

**Error**: `AADSTS65001: The user or administrator has not consented to use the application`

**Solution**:
1. Go to Azure AD → App Registrations → Your App → API permissions
2. Verify **Dynamics CRM → user_impersonation** is added
3. Click **Grant admin consent for [Your Tenant]**
4. Wait 5-10 minutes for propagation
5. Retry connection

### Enable Debug Logging

```csharp
// In appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.PowerPlatform.Dataverse.Client": "Debug"
    }
  }
}
```

---

## ❓ FAQ

### General Questions

**Q: What is the difference between SitecoreAI and Sitecore XM Cloud?**

A: **SitecoreAI** is the AI-powered version of **Sitecore XM Cloud** (the headless CMS). Both terms refer to the same platform - Sitecore's cloud-native, headless content management system.

**Q: Why use Dataverse instead of a traditional database?**

A: Dataverse provides:
- ✅ Built-in Power Platform integration (Power Automate, Power BI, Power Apps)
- ✅ Low-code workflow automation
- ✅ Dynamics 365 native integration
- ✅ Business-user accessible data management
- ✅ Enterprise-grade security and compliance

**Q: Can this work with Sitecore 10.x or earlier versions?**

A: Yes.

### Authentication Questions

**Q: Should I use Client Secret or Certificate authentication?**

A:
- **Client Secret** (Used in this repo): Simpler setup, works with Application Users, easier for beginners
- **Certificate**: More secure for enterprise production, no secret expiration management, preferred for high-security scenarios

Both are production-ready when secrets/certificates are stored in Azure Key Vault.

**Q: Can I use an existing App Registration?**

A: Yes! Just add the **Dynamics CRM → user_impersonation** API permission and create a new client secret for this connector.

### Deployment Questions

**Q: What Azure pricing tier should I use for Functions?**

A:
- **Consumption Plan**: Low traffic, cost-effective ($0.20/million executions)
- **Premium Plan**: High traffic, VNet integration, faster cold starts
- **Dedicated Plan**: Predictable pricing, existing App Service Plan

**Q: How do I monitor Azure Functions in production?**

A: Use **Application Insights** for monitoring, logging, and alerts. Enable in Function App settings.

### Troubleshooting Questions

**Q: Why do I get "field not found" errors with correct field names?**

A: Dataverse logical field names are **case-sensitive** and always **lowercase** in code (e.g., `crf51_id` not `crf51_ID`).

**Q: How do I debug connection issues?**

A: Run `Test-DataverseConnection.ps1` script with detailed output to identify authentication, certificate, or permission issues.

---

## 🤝 Contributing

Contributions welcome! Please follow these guidelines:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### Code Standards

- ✅ Follow C# coding conventions
- ✅ Add XML documentation comments
- ✅ Include unit tests for new features
- ✅ Update README.md if adding functionality

---

## 📚 Resources

### Official Documentation

- [Microsoft Dataverse Documentation](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/)
- [Sitecore XM Cloud Documentation](https://doc.sitecore.com/xmc/)
- [Azure Functions Documentation](https://learn.microsoft.com/en-us/azure/azure-functions/)
- [Microsoft 365 Developer Program](https://developer.microsoft.com/en-us/microsoft-365/dev-program)

### Blog Series

This repository is accompanied by a comprehensive [blog series](https://www.amitk.net/series/sitecoreai-and-microsoft-dataverse-integration/)


### Related Tools

- [PowerShell Dataverse Module](https://www.powershellgallery.com/packages/Microsoft.PowerApps.PowerShell/)
- [XrmToolBox](https://www.xrmtoolbox.com/) - Dataverse management tools
- [Sitecore CLI](https://doc.sitecore.com/xmc/en/developers/xm-cloud/sitecore-command-line-interface.html)

---

## 📄 License

This project is licensed under the **MIT License** - see [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- Microsoft Dataverse team for excellent documentation
- Sitecore community for headless CMS best practices
- Azure Functions team for serverless platform

---

## 📞 Support & Contact

- **Issues**: [GitHub Issues](https://github.com/yourusername/sitecoreai-dataverse-connector/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/sitecoreai-dataverse-connector/discussions)
- **Email**: your.email@example.com

---

### Keywords for Developers

`SitecoreAI` `Sitecore XM Cloud` `Microsoft Dataverse` `Azure Functions` `Power Platform` `Dynamics 365` `Certificate Authentication` `Serverless Integration` `.NET 8` `C#` `MSFD Developer Environment` `Headless CMS` `Form Submission` `CRUD Operations` `Azure AD` `OAuth 2.0` `Production Ready`

---

**⭐ If this repository helped you, please give it a star!**
