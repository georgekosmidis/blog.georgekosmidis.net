## Table of Contents
- [Table of Contents](#table-of-contents)
- [Introduction](#introduction)
  * [Simplifying Distributed App Development](#simplifying-distributed-app-development)
- [Prerequisites](#prerequisites)
- [Quickstart Guide to Building the First .NET Aspire Application](#quickstart-guide-to-building-the-first-net-aspire-application)
  * [Using Visual Studio:](#using-visual-studio-)
  * [Using the .NET CLI:](#using-the-net-cli-)
- [Diving into Orchestration, Components, and Tooling in .NET Aspire](#diving-into-orchestration--components--and-tooling-in-net-aspire)
- [Available .NET Aspire Components](#available-net-aspire-components)
  * [Integrating Components - Aspire.Azure.AI.OpenAI](#integrating-components---aspireazureaiopenai)
- [Deploying .NET Aspire Applications on Azure](#deploying-net-aspire-applications-on-azure)
  * [Deployment Steps:](#deployment-steps-)
- [Integrating with Application Insights for Observability](#integrating-with-application-insights-for-observability)
  * [Integration Steps:](#integration-steps-)
  * [Example Code:](#example-code-)
- [Adding .NET Aspire in an existing app](#adding-net-aspire-in-an-existing-app)
  * [Adding .NET Aspire Orchestration](#adding-net-aspire-orchestration)
  * [Configuring the AppHost](#configuring-the-apphost)
- [Conclusion](#conclusion)
- [Resources](#resources)

## Introduction
.NET Aspire is presented as a forward-thinking, opinionated stack designed specifically for the development of distributed applications that are ready for the cloud environment. It emphasizes the creation of applications that are not only prepared for production but also equipped with built-in observability features. This focus on cloud readiness and observability from the outset demonstrates .NET Aspire's commitment to meeting the demands of modern, distributed application development, making it an essential tool for developers looking to excel in the cloud-native ecosystem.

.NET Aspire significantly simplifies the development of observable, production-ready applications. Its design incorporates best practices and patterns that enable developers to focus on building their application's core functionality while benefiting from the framework's robust support for observability and production readiness. This approach reduces complexity and accelerates the development process, allowing teams to deliver high-quality, cloud-native applications more efficiently.

### Simplifying Distributed App Development

.NET Aspire simplifies the development of distributed applications through its orchestration, components, and tooling by providing a unified and simplified approach to handling common cloud-native concerns:

- **Service Discovery**: .NET Aspire automates the discovery of services within an application, allowing components to communicate seamlessly without hard-coding service locations or details. This simplification is achieved through the orchestration layer, which manages the injection of connection strings and service discovery information.
  
- **Persistence**: Components designed for data storage and retrieval, such as those for PostgreSQL or Redis, offer simplified access to databases and caching systems. They come with standardized configuration patterns, including support for health checks and telemetry, which eases the integration and management of data services in cloud-native applications.

- **Health Checks**: .NET Aspire components can include built-in health checks, ensuring that applications can monitor the status and health of each component and service. This feature is critical for maintaining the reliability and availability of distributed applications, especially in cloud environments where services may be dynamically scaled or replaced.

## Prerequisites
To set up the .NET Aspire environment, follow these prerequisites and steps:
   - Install [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) on your local machine.
   - Install Docker Desktop, as some components, like Redis for caching, require Docker.
   - Have an Integrated Development Environment (IDE) or code editor, such as Visual Studio 2022 (Preview version 17.9 or higher) or Visual Studio Code.
   - For Visual Studio users, Visual Studio 2022 (17.10 Preview 1 or higher) includes the .NET Aspire preview 3 workload by default. You can verify or install this workload via the Visual Studio installer.
   - For users preferring command-line tools or Visual Studio Code, install the .NET Aspire workload using the `.NET CLI` with the command `dotnet workload install aspire`.

## Quickstart Guide to Building the First .NET Aspire Application
To create and run your first .NET Aspire application, you can use either Visual Studio or the .NET CLI. Here's a quick guide for both methods:

### Using Visual Studio:
1. Navigate to `File > New > Project`.
2. Search for "Aspire" in the dialog window, select `.NET Aspire Starter Application`, and click `Next`.
3. Configure your new project by entering a project name (e.g., `AspireSample`) and leave the rest of the values at their defaults. Select `Next`.
4. Ensure `.NET 8.0 (Long Term Support)` is selected, and check `Use Redis for caching (requires Docker)`. Click `Create`.
5. Visual Studio will create a new solution structured to use .NET Aspire, including projects for API services, AppHost, ServiceDefaults, and a Blazor App project.

### Using the .NET CLI:
1. Open a terminal and run the command to create a new .NET Aspire application with Redis cache:
   ```
   dotnet new aspire-starter --use-redis-cache -o AspireAzdWalkthrough
   ```
2. Change into the project directory:
   ```
   cd AspireAzdWalkthrough
   ```
3. Run the project specifying the AppHost project:
   ```
   dotnet run --project AspireAzdWalkthrough.AppHost\AspireAzdWalkthrough.AppHost.csproj
   ```
This set of commands will create and run a new .NET Aspire application based on the `aspire-starter` template, which includes a dependency on Redis cache.

## Diving into Orchestration, Components, and Tooling in .NET Aspire

**Orchestration** in .NET Aspire involves the coordination and management of various elements within a cloud-native application, aiming to streamline the configuration and interconnection of different parts of your application. It provides abstractions for managing service discovery, environment variables, and container configurations without delving into low-level details. This leads to consistent setup patterns across applications with multiple components and service.

Orchestration helps with app composition by specifying .NET projects, containers, executables, and cloud resources that make up the application. It simplifies service discovery and connection string management by managing the injection of the correct connection strings and service discovery information, enhancing the developer experience. 

A practical example includes creating a local Redis container resource and configuring the appropriate connection string in a project with minimal code:

```csharp
// Orchestration example
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Aspire.Orchestration;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register the Redis container as a resource
        services.AddContainerResource("redis", options =>
        {
            options.Image = "redis:latest";
            options.PortMappings.Add("6379:6379");
        });

        // Configure the connection string for Redis
        services.Configure<RedisOptions>(options =>
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            options.ConnectionString = configuration.GetConnectionString("Redis");
        });

        // Other service registrations...
    }
}
```

**Components** in .NET Aspire are NuGet packages designed to simplify connections to popular services like Redis or PostgreSQL. These components handle many cloud-native concerns through standardized configuration patterns, including health checks and telemetry. They are designed to work seamlessly with .NET Aspire's orchestration, allowing configurations to flow through dependencies based on .NET project and package references, thus enabling components to communicate with each other automatically.

For example, the addition of the Azure Service Bus to an application involves registering a `ServiceBusClient` as a singleton in the dependency injection container and applying configurations for health checks, logging, and telemetry specific to Azure Service Bus usage.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Aspire.Orchestration;
using Azure.Messaging.ServiceBus;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register the Service Bus client as a singleton
        services.AddSingleton<ServiceBusClient>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("ServiceBus");

            return new ServiceBusClient(connectionString);
        });

        // Apply configurations for health checks, logging, etc.
        services.AddHealthChecks()
            .AddAzureServiceBusCheck("ServiceBus", provider =>
            {
                var client = provider.GetRequiredService<ServiceBusClient>();
                var queueName = "my-queue";

                return client.CreateReceiver(queueName);
            });

        // Other service registrations...
    }
}
```

**Tooling** provided by .NET Aspire includes project templates and tooling experiences for Visual Studio and the dotnet CLI, which assist in creating and interacting with .NET Aspire applications. These tools follow a standardized structure designed around default .NET Aspire project templates, making it easier to start new projects with a setup that supports the orchestration and use of components.

Visual Studio enhances the experience by offering additional features for working with .NET Aspire components and the App Host orchestrator project. It allows for the easy addition of .NET Aspire components through UI options and supports enlisting in .NET Aspire orchestration during the new project workflow.

## Available .NET Aspire Components

1. **Azure Blob Storage** (`Aspire.Azure.Storage.Blobs`): A library for accessing Azure Blob Storage, facilitating object storage solutions for cloud-native applications.

2. **Azure Cosmos DB Entity Framework Core** (`Aspire.Microsoft.EntityFrameworkCore.Cosmos`): Enables access to Azure Cosmos DB databases using Entity Framework Core, providing a NoSQL database service integration.

3. **Azure Cosmos DB** (`Aspire.Microsoft.Azure.Cosmos`): A library for directly accessing Azure Cosmos DB databases, supporting a wide range of data types and uses.

4. **Azure Key Vault** (`Aspire.Azure.Security.KeyVault`): Facilitates secure access to keys, secrets, and certificates stored in Azure Key Vault, enhancing application security.

5. **Azure Service Bus** (`Aspire.Azure.Messaging.ServiceBus`): Provides capabilities for integrating with Azure Service Bus, a messaging service for connecting applications, services, and systems.

6. **Azure Storage Queues** (`Aspire.Azure.Storage.Queues`): A library for working with Azure Storage Queues, enabling reliable messaging between application components.

7. **Azure Table Storage** (`Aspire.Azure.Data.Tables`): Allows access to Azure Table Storage, offering NoSQL storage for semi-structured data.

8. **MongoDB Driver** (`Aspire.MongoDB.Driver`): Enables MongoDB database access, supporting document-based storage solutions.

9. **MySqlConnector** (`Aspire.MySqlConnector`): Facilitates connectivity to MySQL databases, providing a managed ADO.NET driver.

10. **PostgreSQL Entity Framework Core** (`Aspire.Npgsql.EntityFrameworkCore.PostgreSQL`): Integrates PostgreSQL database access using Entity Framework Core, allowing for object-relational mapping.

11. **PostgreSQL** (`Aspire.Npgsql`): A library for accessing PostgreSQL databases, supporting a wide array of PostgreSQL features.

12. **RabbitMQ** (`Aspire.RabbitMQ.Client`): Enables integration with RabbitMQ, a message broker for complex routing and message queuing.

13. **Redis Distributed Caching** (`Aspire.StackExchange.Redis.DistributedCaching`), **Redis Output Caching** (`Aspire.StackExchange.Redis.OutputCaching`), and **Redis** (`Aspire.StackExchange.Redis`): Provide various levels of caching capabilities using Redis, enhancing performance and scalability.

14. **SQL Server Entity Framework Core** (`Aspire.Microsoft.EntityFrameworkCore.SqlServer`) and **SQL Server** (`Aspire.Microsoft.Data.SqlClient`): Offer integration with SQL Server databases, with and without Entity Framework Core, for relational data storage and manipulation.

### Integrating Components - Aspire.Azure.AI.OpenAI

Let's look at an example, on how to include OpenAI in a .NET Aspire project:

**Integration Steps:**
1. **NuGet Package Installation:** Install the `Aspire.Azure.AI.OpenAI` NuGet package using the .NET CLI command `dotnet add package Aspire.Azure.AI.OpenAI --prerelease`.
2. **Service Registration:** In the `Program.cs` file of your application, use the `builder.AddAzureOpenAI` method to register the OpenAIClient for dependency injection, enabling the application to consume Azure AI OpenAI services.
3. **Configuration:** Configure the OpenAIClient by providing the connection name parameter that corresponds to your Azure OpenAI service configuration, typically defined in your application settings (e.g., `appsettings.json`).

**Example:**
```json
{
  "AzureOpenAI": {
    "ConnectionName": "openAiConnectionName"
  }
}

```

```csharp
// Other service registrations...
services.AddAzureOpenAI(options =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionName = configuration.GetValue<string>("AzureOpenAI:ConnectionName");

    options.ConnectionString = configuration.GetConnectionString(connectionName);
});
// Other configurations...
```

## Deploying .NET Aspire Applications on Azure

.NET Aspire applications are designed with cloud-native principles, allowing for flexible deployment across various platforms, including Azure Container Apps (ACA). ACA provides a fully managed environment for running microservices and containerized applications on a serverless platform, making it an ideal target for .NET Aspire applications.

### Deployment Steps:

1. **Prepare Your .NET Aspire Application**: Ensure your application is containerized and follows .NET Aspire's best practices for cloud-native development.

2. **Azure Container Apps (ACA)**: Utilize ACA for hosting your application. ACA supports manual deployment using Bicep or the Azure Developer CLI (azd), which simplifies the deployment process.

3. **Azure Developer CLI (azd)**: The Azure Developer CLI has been extended to support deploying .NET Aspire applications, offering a guided experience for provisioning and deploying your application to Azure, with specific focus on ACA.

4. **Provisioning and Deployment**: Use the `azd init` and `azd up` commands to provision required Azure resources and deploy your application. The `azd` tool automates the creation of Azure resources like Azure Container Registry and configures your application for deployment to ACA.

5. **Configuration and Environment Setup**: `azd` assists in initializing your project, identifying the .NET Aspire AppHost project, and setting up the environment for deployment. It generates necessary configuration files like `azure.yaml` to describe your app's services and maps them to Azure resources. 

Here's a sample `azure.yaml` configuration that describes how a .NET Aspire application can be configured for deployment using the Azure Developer CLI (azd). This YAML configuration specifies the services of the app, such as the .NET Aspire AppHost project, and maps them to Azure resources, along with an OpenAI service.

```yaml
# yaml-language-server: $schema=https://raw.githubusercontent.com/Azure/azure-dev/main/schemas/v1.0/azure.yaml.json

name: AspireAzdWalkthrough
services:
  app:
    language: dotnet
    project: .\AspireAzdWalkthrough.AppHost\AspireAzdWalkthrough.AppHost.csproj
    host: containerapp
  openai:
    type: external
    api_key: ${OPENAI_API_KEY}
```

## Integrating with Application Insights for Observability

Integrating Application Insights with .NET Aspire applications enhances observability by leveraging Azure Monitor's Application Performance Management (APM) capabilities. This integration allows for a comprehensive understanding of application performance, failures, and usage patterns.

### Integration Steps:

1. **Configure Application Insights**: Start by obtaining the Application Insights connection string from the Azure portal. This connection string should then be configured within your .NET Aspire application, typically as an environment variable within the AppHost project. This ensures the connection string is included in the deployment manifest for seamless integration.

2. **Telemetry Data Collection**: .NET Aspire applications are designed with OpenTelemetry in mind, enabling the configuration of telemetry data collection to be directed towards Application Insights. This setup provides valuable insights into application performance and operational events.

3. **Use Azure Monitor Exporter**: Include the Azure Monitor OpenTelemetry Exporter package in your application to facilitate the process of sending telemetry data to Application Insights. This package provides a set of defaults that enhance the observability of your application by simplifying the export of telemetry data to Azure Monitor.

### Example Code:

To integrate Application Insights, you can configure the connection string and use the Azure Monitor Exporter as follows:

```csharp
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var appInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

var cache = builder.AddRedisContainer("cache");

var apiservice = builder.AddProject<Projects.AspireApp_ApiService>("apiservice")
    .WithEnvironment("APPLICATIONINSIGHTS_CONNECTION_STRING", appInsightsConnectionString);

builder.AddProject<Projects.AspireApp_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice)
    .WithEnvironment("APPLICATIONINSIGHTS_CONNECTION_STRING", appInsightsConnectionString);

builder.Build().Run();
```

And to manage the connection string securely:

```json
{
  "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=12345678-abcd-1234-abcd-1234abcd5678;IngestionEndpoint=https://westus3-1.in.applicationinsights.azure.com"
}
```

Lastly, include the Azure Monitor Exporter package in your `ServiceDefaults` project to ensure it is available across all .NET Aspire applications:

```xml
<PackageReference Include="Azure.Monitor.OpenTelemetry.AspNetCore" Version="[SelectVersion]" />
```

This integration approach outlines the critical steps for deploying .NET Aspire applications to Azure, utilizing ACA for hosting, and enhancing telemetry and observability with Application Insights. It underscores the synergy between Azure services and .NET Aspire, promoting a cloud-native development paradigm while ensuring robust monitoring and analytical capabilities essential for cloud-based applications.

## Adding .NET Aspire in an existing app
To add .NET Aspire to an existing .NET 8 microservices application, follow these steps to harness its benefits of .NET Aspire, including service orchestration and enhanced telemetry:

### Adding .NET Aspire Orchestration

1. **Enroll the Store Project**: Right-click on your main project in Solution Explorer, select `Add`, and then choose `.NET Aspire Orchestrator Support`. Confirm any dialogs that appear to proceed with the addition.

2. **Enroll Other Projects**: Similarly, add .NET Aspire support to other projects within your solution that you wish to be part of the orchestration. This might include backend services, APIs, or any other project types relevant to your application.

After adding .NET Aspire to your projects, Visual Studio will automatically add two new projects to your solution:
- `AppHost`: An orchestrator project to connect and configure the different projects and services.
- `ServiceDefaults`: A shared project to manage configurations reused across the solution, related to resilience, service discovery, and telemetry.

### Configuring the AppHost

1. **Configure Service Registration**: In the `Program.cs` file of your AppHost project, register the projects you've added to .NET Aspire orchestration using the `services.AddProject<Projects.YourProject>("yourProject")` method, where `YourProject` is the project name, and `"yourProject"` is the identifier used in the orchestration.

2. **Start the Application**: Debug the solution in Visual Studio to see the effects of .NET Aspire orchestration. Ensure Docker Desktop is running, as it might be required for services that use containerization.

3. **Verify Orchestration and Service Discovery**: Access the .NET Aspire dashboard that appears upon running the application to verify that your services are correctly registered and discoverable within the orchestration framework.

By following these steps, you integrate .NET Aspire into your existing microservices application, leveraging its orchestration capabilities, enhanced security, and observability features.

## Conclusion
.NET Aspire has emerged as a transformative stack for cloud-native application development, offering tools and patterns that align with the needs of modern distributed systems. Through a blend of orchestration, componentization, and comprehensive tooling, it streamlines the creation of observable, production-ready applications, setting developers on a path from novice to expert in the cloud-native landscape. By leveraging .NET Aspire, developers can focus on delivering value through their applications, confident in the stack's capabilities to handle the complexities of cloud-native development efficiently. This journey encapsulates the essence of evolving with .NET Aspire: embracing innovation, enhancing skills, and contributing to the future of cloud-ready applications.

## Resources
For those interested in .NET Aspire and looking for related content, here are some useful links to GitHub repositories and documentation:

1. **.NET Aspire Repository**: This is the main GitHub repository for .NET Aspire, where you can find all related components, documentation, and contribution guidelines.
   - [GitHub - dotnet/aspire](https://github.com/dotnet/aspire)

2. **.NET Aspire Samples**: A repository dedicated to providing samples for .NET Aspire, showcasing various applications and implementations to help you get started.
   - [GitHub - dotnet/aspire-samples](https://github.com/dotnet/aspire-samples)

3. **.NET Aspire Documentation**: This repository contains the documentation for .NET Aspire, offering in-depth information on how to use, extend, and deploy .NET Aspire applications.
   - [GitHub - dotnet/docs-aspire](https://github.com/dotnet/docs-aspire)

