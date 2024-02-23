## Table of Contents

- [Table of Contents](#table-of-contents)
- [Introduction to Serverless Computing](#introduction-to-serverless-computing)
- [Serverless Computing on Azure](#serverless-computing-on-azure)
  * [Key Features of Azure Functions:](#key-features-of-azure-functions-)
  * [Example: A Simple HTTP-triggered Azure Function](#example--a-simple-http-triggered-azure-function)
- [Durable Serverless Computing on Azure](#durable-serverless-computing-on-azure)
  * [Why Durable Functions?](#why-durable-functions-)
  * [Key Concepts](#key-concepts)
  * [Example: Order Processing Workflow](#example--order-processing-workflow)
- [Core Concepts of Azure Durable Functions](#core-concepts-of-azure-durable-functions)
  * [Function Chaining: How to Execute a Sequence of Functions in a Specific Order](#function-chaining--how-to-execute-a-sequence-of-functions-in-a-specific-order)
  * [Fan-out/Fan-in: Implementing Parallel Processing Patterns and Aggregating Results](#fan-out-fan-in--implementing-parallel-processing-patterns-and-aggregating-results)
  * [Async HTTP APIs: Building Long-Running Processes with HTTP Endpoints](#async-http-apis--building-long-running-processes-with-http-endpoints)
  * [Monitoring: Creating Workflows that Monitor the Status of External Services](#monitoring--creating-workflows-that-monitor-the-status-of-external-services)
  * [Human Interaction: Managing Workflows That Require Human Intervention or Approval](#human-interaction--managing-workflows-that-require-human-intervention-or-approval)
  * [Resources and Further Reading](#resources-and-further-reading)

## Introduction to Serverless Computing
Serverless computing is a cloud computing execution model where the cloud provider dynamically manages the allocation and provisioning of servers. A serverless application runs in stateless compute containers that are event-triggered, ephemeral (may last for one invocation), and fully managed by the cloud provider. This means developers can focus on their code without worrying about the underlying infrastructure.

The primary benefits of serverless computing include:
- **Cost Efficiency**: You only pay for the compute time you consume, down to the nearest 100 milliseconds, making it highly cost-effective.
- **Scalability**: Automatically scales your application by running code in response to each trigger.
- **Simplified Operations**: Eliminates the need to manage servers, leading to faster development cycles.

## Serverless Computing on Azure
Azure Functions is Microsoft's answer to serverless computing. It allows developers to write less code, maintain less infrastructure, and save on costs. In essence, Azure Functions is a serverless compute service that enables you to run code on-demand without having to explicitly provision or manage infrastructure.

### Key Features of Azure Functions:
- **Event-driven**: You can trigger functions from a variety of events, including HTTP requests, database operations, queue messages, and more.
- **Integrated Security**: Protect your functions with OAuth providers such as Azure Active Directory, Facebook, Google, Twitter, and Microsoft Account.
- **Programming Language Support**: Write functions using your choice of C#, Java, JavaScript, TypeScript, and Python.
- **Scalability**: Azure Functions scale automatically based on demand, so your code always has the resources it needs to run, but you're only charged for the exact amount of resources your functions use.

### Example: A Simple HTTP-triggered Azure Function 
Let’s look at a simple example of an Azure Function that's triggered by an HTTP request. This function (which is obviously **not** a Durable Function!) will return a personalized greeting to the user.

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public static class GreetFunction
{
    [FunctionName("GreetFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
    {
        string name = req.Query["name"];

        string responseMessage = string.IsNullOrEmpty(name)
            ? "This HTTP triggered function executed successfully. Pass a name in the query string for a personalized response."
            : $"Hello, {name}. This HTTP triggered function executed successfully.";

        return new OkObjectResult(responseMessage);
    }
}
```

In this example:
- The `HttpTrigger` attribute indicates that the function is triggered by an HTTP request.
- The function checks for a `name` query parameter and uses it to generate a personalized greeting. If no name is provided, it returns a generic message.
- The function returns an `OkObjectResult`, which is an HTTP 200 OK response, containing the greeting message.

## Durable Serverless Computing on Azure
Durable Functions is an extension of Azure Functions that enables you to write stateful functions in a serverless computing environment. This extension allows for the creation of complex orchestration workflows where functions can call other functions, wait for those functions to finish, and resume where they left off without maintaining state externally. Durable Functions simplifies the process of writing stateful applications in a stateless environment.

### Why Durable Functions?
- **State Management**: Automatically manages state persistence, enabling you to focus on business logic rather than data storage.
- **Complex Workflows**: Simplifies the development of complex workflows, including sequential and parallel execution patterns.
- **Durability and Reliability**: Ensures execution even in the event of unexpected failures, with built-in retry policies and error handling.
- **Serverless Benefits**: Inherits the scalability, flexibility, and cost-efficiency of Azure Functions.

### Key Concepts
- **Orchestrator Functions**: Control the workflow, executing other functions in a sequence or parallel, making decisions, and managing state.
- **Activity Functions**: Perform the actual work of the workflow, such as database operations, calls to external services, or any computational task.
- **Client Functions**: Trigger orchestrations and inquire about the status of a running orchestration.

### Example: Order Processing Workflow
Imagine a scenario where you need to process orders in an e-commerce system. The workflow involves validating the order, checking inventory, processing payment, and finally, sending a confirmation email to the customer.

#### Step 1: Define the Orchestrator Function
The orchestrator function coordinates the entire process. It calls activity functions for each step of the process and waits for their completion.

```csharp
[FunctionName("ProcessOrderOrchestrator")]
public static async Task<object> RunOrchestrator(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    var order = context.GetInput<Order>();

    // Validate the order
    bool isValid = await context.CallActivityAsync<bool>("ValidateOrder", order);

    if (isValid)
    {
        // Check inventory
        bool isInStock = await context.CallActivityAsync<bool>("CheckInventory", order);
        
        if (isInStock)
        {
            // Process payment
            bool paymentSuccess = await context.CallActivityAsync<bool>("ProcessPayment", order);
            
            if (paymentSuccess)
            {
                // Send confirmation email
                await context.CallActivityAsync("SendConfirmationEmail", order);
                
                return "Order processed successfully!";
            }
        }
    }

    return "Order processing failed.";
}
```

#### Step 2: Implement Activity Functions
Each step in the process is implemented as an activity function. For example, here's a simplified version of the `ValidateOrder` activity function:

```csharp
[FunctionName("ValidateOrder")]
public static bool ValidateOrder([ActivityTrigger] Order order, ILogger log)
{
    // Logic to validate the order
    return true; // Assume the order is valid for this example
}
```

Each activity function (`ValidateOrder`, `CheckInventory`, `ProcessPayment`, `SendConfirmationEmail`) would be implemented similarly, performing its specific task.

#### Step 3: Trigger the Workflow
A client function triggers the orchestrator function, starting the workflow. The client could be an HTTP-triggered function that receives order submissions.

## Core Concepts of Azure Durable Functions

Azure Durable Functions stand out as a highly potent extension of Azure Functions, enabling developers to craft stateful functions in a serverless compute environment. This unique capability allows for the orchestration of complex workflows, asynchronous operations, and long-running processes without the overhead of managing server state or worrying about the durability of the workflow's progress. To fully leverage the power of Durable Functions, it’s essential to grasp a few core concepts that underpin how these functions operate and interact within the Azure ecosystem. Understanding these concepts will empower developers to design and implement more efficient, scalable, and reliable serverless applications.

### Function Chaining: How to Execute a Sequence of Functions in a Specific Order

Function chaining in Azure Durable Functions is a pattern that allows you to execute a series of functions in a particular sequence. This pattern is particularly useful when you have tasks that need to be performed in order, where each task starts only after the previous one has completed, and the output of one function becomes the input to the next. Here’s a deeper dive into how function chaining works and how to implement it effectively.

#### Understanding Function Chaining

Function chaining involves invoking functions in a sequence, with each function performing its task and passing the result to the next function in the chain. This sequential execution is critical for workflows that require tasks to be completed in a specific order due to dependencies between tasks.

#### How Durable Functions Facilitate Function Chaining

Durable Functions extend Azure Functions by enabling stateful execution in a serverless environment. This statefulness is crucial for function chaining, as it allows the workflow to remember the state of execution as it moves from one function to the next. Durable Functions use "orchestrator functions" to control the workflow, including the execution order of functions.

#### Implementing Function Chaining

1. **Define Your Functions**: Start by defining the Azure Functions that will form the links in your chain. Each function should perform a discrete task.

2. **Create an Orchestrator Function**: The orchestrator function is responsible for controlling the sequence in which your functions are called. You define the logic of the function chaining in this orchestrator.

3. **Use `CallActivityAsync`**: Inside the orchestrator function, use the `CallActivityAsync` method to call each function in the sequence. This method asynchronously calls another function, waits for its completion, and retrieves the result. The syntax is straightforward: `var result = await context.CallActivityAsync<ReturnType>("FunctionName", input);` where `ReturnType` is the type of result expected from the function, `"FunctionName"` is the name of the function to call, and `input` is any input the function requires.

4. **Pass Results Along**: After a function completes, its return value can be passed as input to the next function in the chain. This is done within the orchestrator function, allowing you to control the flow of data between functions.

#### Example Scenario

Imagine a workflow where you need to process an order, charge a payment, and then send a confirmation email. You would create three functions: `ProcessOrder`, `ChargePayment`, and `SendConfirmationEmail`. Your orchestrator function would call these functions in sequence, passing the necessary data from one step to the next.

```csharp
[FunctionName("OrderWorkflow")]
public static async Task Run(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    var orderDetails = context.GetInput<OrderDetails>();

    // Process the order
    var processedOrder = await context.CallActivityAsync<OrderDetails>("ProcessOrder", orderDetails);

    // Charge payment
    var paymentResult = await context.CallActivityAsync<PaymentResult>("ChargePayment", processedOrder);

    // Send confirmation email
    await context.CallActivityAsync("SendConfirmationEmail", paymentResult);
}
```

In this example, each function is called in order, with the output of one function serving as the input for the next. This ensures that the order is processed, paid for, and confirmed in a sequence that respects the logical flow of the application.

#### Best Practices

- **Error Handling**: Implement try-catch blocks within your orchestrator function to handle errors gracefully. This is crucial for maintaining the integrity of your workflow, especially in long chains where one function's failure could impact subsequent operations.
- **Idempotency**: Ensure that your functions are idempotent, meaning they can be called multiple times without changing the result beyond the initial call. This is important for retry policies and error recovery in durable functions.

Function chaining is a powerful pattern for orchestrating complex workflows in Azure Durable Functions. By understanding and implementing this pattern, you can build robust, sequential workflows that leverage the full potential of serverless computing in Azure.

### Fan-out/Fan-in: Implementing Parallel Processing Patterns and Aggregating Results

When dealing with complex workflows in serverless architectures, efficiency and scalability are key. The "Fan-out/Fan-in" pattern is a powerful strategy in Durable Functions that enables you to execute multiple functions in parallel and then aggregate their results. This pattern is particularly useful for tasks that can be processed independently, allowing for significant performance improvements over sequential processing.

#### Understanding the Fan-out/Fan-in Pattern

**Fan-out** refers to the process of executing multiple operations in parallel. Rather than running tasks one after another, the Fan-out pattern disperses them across multiple instances, allowing them to run concurrently. This is particularly useful for operations like batch processing, data analysis, or any scenario where tasks are not dependent on the outcome of others.

**Fan-in** is the subsequent step where the results of these parallel operations are collected and aggregated. Once all parallel tasks have completed, the Fan-in process consolidates their outcomes into a single result. This could be a summary, a combined dataset, or any form of aggregation that supports the workflow's objectives.

#### Implementing Fan-out/Fan-in in Durable Functions

Durable Functions simplifies the implementation of the Fan-out/Fan-in pattern through its orchestration capabilities. Here's a step-by-step guide to leveraging this pattern:

1. **Orchestration Function**: Start with an orchestration function that defines the workflow. This function will coordinate the fan-out to parallel activities and the subsequent fan-in of results.

2. **Fan-out**: Use the `CallActivityAsync` method within a loop or parallel tasks to initiate multiple instances of an activity function. These instances run concurrently, achieving the fan-out effect.

3. **Collect Results**: As each activity function completes, its result is returned to the orchestration function. Store these results in a collection, such as a list or an array.

4. **Fan-in**: Once all parallel tasks have completed, proceed to the fan-in phase. Aggregate the collected results into a final outcome. This could involve summing numbers, concatenating strings, merging datasets, etc.

5. **Return Aggregate Result**: The orchestration function returns the aggregated result, concluding the workflow.

#### Best Practices and Considerations

- **Concurrency Limits**: Be aware of the concurrency and throughput limits of your environment. Excessive parallelism can lead to throttling or increased costs.
- **Error Handling**: Implement comprehensive error handling for individual tasks. Failures in one task should not compromise the entire operation.
- **State Management**: Durable Functions efficiently manages state for you, but be mindful of the data being passed between functions to avoid performance bottlenecks.

This pattern is ideal for scenarios where tasks can be executed concurrently, significantly reducing processing time compared to sequential execution.

#### A simple example

To implement this pattern in Durable Functions, you'll primarily work with an orchestrator function. The orchestrator function is responsible for coordinating the execution of activity functions (the tasks that will run in parallel) and aggregating their results.

Here's a simple example:

```csharp
[FunctionName("FanOutFanInOrchestrator")]
public static async Task<List<string>> RunOrchestrator(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    var outputs = new List<string>();

    // Assume we have a list of tasks to process in parallel
    var tasksToProcess = new List<string> { "Task1", "Task2", "Task3" };

    // Fan-out: Initiating parallel tasks
    var parallelTasks = new List<Task<string>>();
    foreach (var task in tasksToProcess)
    {
        Task<string> taskOperation = context.CallActivityAsync<string>("ProcessTask", task);
        parallelTasks.Add(taskOperation);
    }

    // Fan-in: Waiting for all tasks to complete and aggregating results
    await Task.WhenAll(parallelTasks);
    foreach (var completedTask in parallelTasks)
    {
        outputs.Add(await completedTask);
    }

    // Outputs contains the aggregated results of all completed tasks
    return outputs;
}

[FunctionName("ProcessTask")]
public static string ProcessTask([ActivityTrigger] string task, ILogger log)
{
    // Simulate task processing
    log.LogInformation($"Processing {task}");
    return $"{task} completed";
}
```

#### Explanation

- **Orchestrator Function (`FanOutFanInOrchestrator`)**: This function orchestrates the workflow. It starts by creating a list of tasks (`tasksToProcess`). For each task, it initiates an asynchronous operation by calling the `ProcessTask` activity function. These operations are executed in parallel (fan-out). After initiating all tasks, it waits for all of them to complete using `Task.WhenAll` and aggregates the results (fan-in).

- **Activity Function (`ProcessTask`)**: Represents a single unit of work that is executed as part of the parallel operations. In this example, it simply logs the processing of a task and returns a string indicating completion.


### Async HTTP APIs: Building Long-Running Processes with HTTP Endpoints

Durable Functions provide an elegant solution for handling long-running processes in serverless applications. One of the standout features is the ability to create asynchronous HTTP APIs, which are crucial for tasks that require more time to complete than a standard HTTP request/response cycle allows. This feature is especially useful for processes such as data processing, batch jobs, or any task that may take an unpredictable amount of time to complete.

#### The Challenge with Long-Running HTTP Requests

In traditional HTTP interactions, a client sends a request to a server, which processes the request and returns a response. This synchronous model assumes that the processing time is short, allowing the client to remain connected until the response is received. However, long-running processes challenge this assumption, leading to timeouts or a poor user experience as the client waits for a response.

#### How Durable Functions Address This

Durable Functions introduce a pattern that decouples the long-running task from the initial HTTP request, using status query endpoints, send asynchronous response techniques, and external event handling.

##### 1. Starting the Process

When an HTTP request initiates a long-running process, the Durable Function orchestrator function starts the task and immediately responds to the client with an HTTP 202 Accepted status. This response includes URLs for status querying and sending external events to the process, if necessary.

```csharp
[FunctionName("StartProcess")]
public static async Task<HttpResponseMessage> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
    [DurableClient] IDurableOrchestrationClient starter,
    ILogger log)
{
    // Parse request and start a new orchestration
    var instanceId = await starter.StartNewAsync("MyOrchestration", null);

    // Return a response with status endpoint
    var response = starter.CreateCheckStatusResponse(req, instanceId);
    response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(10));
    return response;
}
```

##### 2. Checking Process Status

The client can use the provided status query endpoint to check the progress of the task. This endpoint returns the current state of the process, including running, completed, or failed, along with any outputs if the process has completed.

##### 3. Handling Long Processing Times

For processes that take an extended period, Durable Functions leverage durable timers to pause the orchestrator function, freeing up resources while waiting. The orchestrator function can resume once the external event is triggered or the timer completes, ensuring efficient resource utilization.

#### Benefits of Async HTTP APIs in Durable Functions

- **Scalability**: By decoupling the execution from the client request, resources are used more efficiently, allowing for greater scalability.
- **Resiliency**: The stateful nature of Durable Functions means that long-running processes can withstand failures and continue from their last known state.
- **Flexibility**: Clients are not tied up waiting for a response and can check back at their convenience or be notified upon completion.


### Monitoring: Creating Workflows that Monitor the Status of External Services

In the realm of serverless architectures, maintaining awareness of your external services' health and performance is crucial. Durable Functions, an extension of Azure Functions, provides a robust framework for building complex, stateful workflows in a serverless environment. A particularly powerful application of Durable Functions is creating workflows designed to monitor the status of external services continuously.

#### The Basics of Monitoring with Durable Functions

Monitoring workflows in Durable Functions are typically implemented using the "Eternal Orchestrations" pattern. This pattern allows an orchestration to run indefinitely, invoking itself after a specified delay, thus creating a continuous loop. This approach is particularly suited for monitoring tasks, where the workflow needs to poll an external service at regular intervals to check its status.

#### Implementing a Monitoring Workflow

Here's a high-level approach to implementing a monitoring workflow using Durable Functions:

1. **Orchestration Function Setup**: Begin by defining an orchestration function. This function will act as the central coordinator for your monitoring workflow. It will call activity functions to perform specific tasks, such as checking the service's status and sending alerts if necessary.

2. **Activity Function for Service Check**: Implement an activity function dedicated to querying the external service's status. This could involve making HTTP requests to a status endpoint, checking for specific responses, or validating the service's response time against predefined thresholds.

3. **Scheduling Checks**: Within the orchestration function, use the `CreateTimer` method to schedule the next service check. This method enables you to specify a delay, effectively determining how frequently your workflow polls the external service.

4. **Handling Service Status**: Based on the response from your activity function, implement logic within your orchestration to handle various service states. For example, if the service is down, you might initiate another activity function to send an alert to your operations team.

5. **Looping for Continuous Monitoring**: To create an eternal orchestration, make sure the orchestration function calls itself after completing each cycle of service checks and handling. This self-invocation, combined with a delay set by `CreateTimer`, ensures that the monitoring workflow runs continuously.

#### Best Practices

- **Idempotency**: Ensure that your activity functions are idempotent. Since your monitoring workflow will repeatedly invoke these functions, idempotency ensures that repeated executions do not lead to unintended consequences.
- **Scalability**: Consider the scalability of your workflow. Durable Functions are designed to scale automatically, but monitoring highly available services may require adjustments to function timeouts and concurrency settings.
- **Error Handling**: Implement comprehensive error handling within your workflow. This includes handling transient errors gracefully and defining clear escalation paths for persistent issues with the external service.
- **Resource Optimization**: Be mindful of the costs associated with polling external services frequently. Optimize the frequency of checks to balance between timely awareness and resource consumption.

Certainly! Let's add a practical example to the "Monitoring: Creating workflows that monitor the status of external services" section to illustrate how you might implement a basic monitoring workflow using Durable Functions in Azure.


#### Practical Example: Monitoring an External API Service

In this example, we'll create a simple Durable Function workflow that monitors the availability of an external API service by periodically sending HTTP requests to check its health endpoint.

##### Step 1: Setup the Orchestration Function

First, define your orchestration function. This function will orchestrate the monitoring process, including scheduling checks and handling the results.

```csharp
[FunctionName("MonitorExternalServiceOrchestrator")]
public static async Task RunOrchestrator(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    DateTime nextCheck = context.CurrentUtcDateTime.AddMinutes(5); // Schedule the next check in 5 minutes
    await context.CallActivityAsync("CheckExternalServiceStatus", null);
    await context.CreateTimer(nextCheck, CancellationToken.None); // Wait until the next check

    context.ContinueAsNew(null); // Continue the orchestration indefinitely
}
```

##### Step 2: Implement the Activity Function to Check Service Status

Create an activity function that performs the actual check on the external service's health endpoint.

```csharp
[FunctionName("CheckExternalServiceStatus")]
public static async Task<bool> CheckServiceStatus([ActivityTrigger] IDurableActivityContext context, ILogger log)
{
    var httpClient = new HttpClient();
    var response = await httpClient.GetAsync("https://your-external-service.com/health");
    bool isServiceUp = response.IsSuccessStatusCode;

    if (!isServiceUp)
    {
        // Log or handle the service being down (e.g., send an alert)
        log.LogError("External service is down.");
    }
    else
    {
        log.LogInformation("External service is up.");
    }

    return isServiceUp;
}
```

By adapting this example, you can monitor virtually any external service or endpoint, customizing the check frequency, handling logic, and alerting mechanisms to suit your specific requirements.

### Human Interaction: Managing Workflows That Require Human Intervention or Approval

Durable Functions, an extension of Azure Functions, allows developers to build complex orchestration workflows in a serverless environment. A significant capability of these orchestrations is managing processes that require human intervention or approval. This functionality is crucial for workflows where a decision or action by a person is needed before the process can proceed.

#### Understanding Human Interaction in Workflows

Human interactions in automated workflows typically involve pausing the workflow until an external input is received. This could be an approval from a manager, feedback from a client, or a manual review of generated content. Durable Functions handle these scenarios through **external events**.

#### Implementing Human Interaction

To implement a workflow that requires human intervention in Durable Functions:

1. **Start with an Orchestrator Function**: This function coordinates the workflow, including the wait for human input.
   
2. **Use External Events for Pausing and Resuming**: The orchestrator function can pause its execution waiting for an external event. This event represents the human action, such as an approval.
   
3. **Sending Approval Requests**: Typically, an activity function sends an approval request to a human via email or a web interface. This message includes a way to send the approval back, often through a secure link that triggers another Azure Function.

4. **Waiting for Approval**: The orchestrator function waits for the external event triggered by human action. This is done using the `waitForExternalEvent` method, which effectively pauses the workflow until the specified event is received.

5. **Resuming the Workflow**: Once the external event (e.g., approval) is received, the orchestrator function continues, executing the next steps in the workflow based on the input received from the human interaction.

#### Handling Timeouts

In real-world scenarios, human actions may not be immediate. Durable Functions allows setting a timeout for the wait. If the human action is not received within the timeout period, the workflow can proceed with a default action, escalate the issue, or retry the request.

#### Example Scenario: Document Approval Process

Consider a document approval process where a document generated by a system needs to be reviewed and approved by a manager before it is finalized:

- **Step 1**: The orchestrator function starts the workflow, generating the document and sending a notification to the manager for approval.
- **Step 2**: The workflow waits for the manager's approval, pausing execution until an external event signals the manager's decision.
- **Step 3**: Upon receiving approval (the external event), the workflow resumes, finalizing the document and proceeding to the next steps, such as notifying stakeholders or archiving the document.

### Resources and Further Reading

#### Official Documentation and Tutorials
- Azure Durable Functions Documentation: [docs.microsoft.com/azure/azure-functions/durable/durable-functions-overview](https://docs.microsoft.com/azure/azure-functions/durable/durable-functions-overview)
- Microsoft Learn - Durable Functions Module: [docs.microsoft.com/learn/modules/create-long-running-serverless-workflow-with-durable-functions](https://docs.microsoft.com/learn/modules/create-long-running-serverless-workflow-with-durable-functions)

#### Community Resources, Forums, and Blogs
- GitHub Repository for Durable Functions: [github.com/Azure/azure-functions-durable-extension](https://github.com/Azure/azure-functions-durable-extension)
- Stack Overflow - Azure Functions Durable Tag: [stackoverflow.com/questions/tagged/azure-functions-durable](https://stackoverflow.com/questions/tagged/azure-functions-durable)
- Azure Blog: [azure.microsoft.com/blog/topics/azure-functions](https://azure.microsoft.com/blog/topics/azure-functions)

#### YouTube Channels and Webinars
- Microsoft Azure YouTube Channel: [youtube.com/user/windowsazure](https://www.youtube.com/user/windowsazure)
- Azure DevOps Labs: [youtube.com/channel/UC-ikyViYMM69joIAmZCnepg](https://www.youtube.com/channel/UC-ikyViYMM69joIAmZCnepg)

#### Books and eBooks
- "Pro Azure Functions" for insights including Durable Functions
