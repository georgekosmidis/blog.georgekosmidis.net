
## Introduction

The evolution of Artificial Intelligence and its integration into development frameworks has significantly enhanced the capabilities of applications in data processing and analysis. Semantic Kernel, with its extensibility and compatibility with models from OpenAI, Azure OpenAI, Hugging Face, and soon to include more, offers a unique opportunity for .NET developers to elevate their applications. This post dives into the concept of Semantic Kernel, its significance, and practical examples of its application in Semantic Understanding.

## Semantic Kernel

Semantic Kernel is a lightweight, open-source Software Development Kit (SDK) designed to enable the integration of AI capabilities with existing code through plugins. This orchestration allows developers to automate tasks and process data intelligently within their .NET applications. By leveraging Semantic Kernel, developers can describe their code to AI models, enabling these models to call the described functions, thereby achieving a seamless integration of AI capabilities into existing applications.

With that said, one might wonder how an AI model can call my C# or Python scripts? The answer, in short, is Semantic Understanding!

## Semantic Understanding

Semantic understanding, in the context of integrating AI models with existing code through platforms like Semantic Kernel, refers to the ability of the system to comprehend the purpose, capabilities, and requirements of a piece of code based on its descriptions. This understanding goes beyond just recognizing code syntax; it involves grasping the functional role that the code plays within an application, much like understanding the meaning of words and sentences in human language. 

In the context of Semantic Kernel, Semantic Understanding allows AI models to interact intelligently with existing codebases. It simplifies the process of building AI-powered applications by providing a layer that translates between AI model requests and the actions that application code can perform. This enables developers to focus on what their code does rather than how it integrates with AI, fostering innovation and easing the incorporation of AI into software solutions.

### How Semantic Understanding is Achieved

1. **Annotations and Descriptions**: Developers annotate their code with metadata that describes what the code does, its inputs, outputs, and side effects. These annotations act like comments that are meant to be understood by the Semantic Kernel rather than by humans or the compiler.

2. **Exposing Capabilities**: By describing functions and methods in this way, the code effectively communicates its capabilities to the Semantic Kernel, enabling it to "know" what actions it can perform with the code. For example, a function that sends emails would be annotated to indicate that it can send messages to specified email addresses.

3. **AI Model Requests**: When AI models (such as chatbots, assistants, or decision-making systems) require performing an action, they issue requests in natural language or structured commands. The Semantic Kernel uses its semantic understanding of the code to map these requests to the appropriate pieces of annotated code.

> Two decorations can be used on your methods and attributes to achieve Semantic Understanding:
> - `[KernelFunction]` marks a method as callable by the Semantic Kernel. It indicates that the method can be invoked as part of the AI's decision-making process or in response to a query. This is crucial for integrating custom logic or external services into the Semantic Kernel's execution flow. 
> - `[Description(string)]` provides metadata about the method it decorates. 

### How Semantic Understanding is Used

1. **Automated Task Execution**: Semantic understanding allows Semantic Kernel to automatically select and execute pieces of code in response to AI model requests. For instance, if an AI model needs to retrieve data, it doesn't need to know how to query a database. Instead, it requests the data it needs, and Semantic Kernel executes the corresponding described code.

2. **Dynamic Orchestration**: The system can dynamically orchestrate complex workflows involving multiple steps and different pieces of code based on its understanding. This orchestration is guided by the semantic descriptions of capabilities, making it possible to adapt to various tasks without hardcoded integrations.

3. **Enhanced Interoperability**: Semantic understanding enhances the interoperability between disparate systems and AI models. By understanding the semantics of different pieces of code, Semantic Kernel can bridge the gap between them, enabling them to work together seamlessly.

4. **Simplifying AI Integration**: Developers can integrate sophisticated AI functionalities into their applications without deep knowledge of AI model internals. They focus on describing what their code does, and Semantic Kernel handles the rest, making AI integration accessible to a broader range of developers.

5. **Facilitating Responsible AI Usage**: With a deeper understanding of what code does, Semantic Kernel can help ensure that AI models are used responsibly. It can prevent certain actions based on context or user preferences, aligning AI behavior with ethical guidelines and user expectations.

But let's explore all that with a common scenario, let's illustrate how Semantic Kernel utilizes semantic understanding through a simplified example, focusing on an AI-powered application that automates customer support by categorizing and responding to user queries. 

### Example: AI-Powered Customer Support 

#### Step 1: Decorating code

Imagine we have a web application that receives various customer support queries. Some queries are about technical issues ("I'm having trouble logging in"), while others are about billing ("I was overcharged this month"). We want to use Semantic Kernel to automatically categorize these queries and provide a standardized response based on the category. To solve this, lets assume we already have the following two objects that act as solution entry points for the two different request types.

```csharp
public class HandleTechnicalIssuePlugin
{
    // Handles technical support queries
    public string HandleTechnicalIssue(string query)
    {
        // Logic to handle technical query
        // ...
    }
}

public class HandleBillingIssuePlugin
{
    // Handles billing support queries
    public string HandleBillingIssue(string query)
    {
        // Logic to handle billing query
        // ...
    }
}
```

To enhance our application's capability with Semantic Understanding, we integrate Semantic Kernel by decorating methods and arguments with the attributes we discussed already:


```
public class HandleTechnicalIssuePlugin
{
    [KernelFunction]
    [Description("Provides assistance for technical-related customer queries.")]
    public string HandleTechnicalIssue([Description("A detailed description of the technical issue faced by the customer.")] string query)
    {
        // Logic to handle technical query
        return "Have you tried turning it off and on again?"
    }
}

public class HandleBillingIssuePlugin
{
    [KernelFunction]
    [Description("Resolves queries related to billing, payments, and account charges.")]
    public string HandleBillingIssue([Description("A detailed description of the billing issue encountered by the customer.")] string query)
    {

        // Logic to handle billing query
        return "Too late, we have your credit card now..."
    }
}

```


#### Step 2: Register the kernel


In the `Startup.cs` or `Program.cs` file where you configure your services, add the following lines of code

```csharp
    // Initialize Semantic Kernel and register plugins as needed
    var kernelBuilder = Kernel.CreateBuilder();
    
    // Add the two plugins
    kernelBuilder.Plugins.AddFromType<HandleTechnicalIssue>();
    kernelBuilder.Plugins.AddFromType<HandleBillingIssue>();
    
    // Configure additional services like AzureOpenAIChatCompletion
    kernelBuilder.AddAzureOpenAIChatCompletion("deploymentName", "endpoint", "apiKey");

    // Build and register the kernel as a singleton service
    var kernel = kernelBuilder.Build();
    services.AddSingleton(kernel);
```

#### Step 3: Create a Controller 

To handle incoming HTTP requests and efficiently manage them, you will need to create a controller in your .NET application. This controller will act as an intermediary, receiving user queries through HTTP requests and using Semantic Kernel to process these queries and return appropriate responses. 

Here’s how you can set it up:


```csharp
using Microsoft.AspNetCore.Mvc;
using SemanticKernelExample.Services;

namespace SemanticKernelExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly Kernel _kernel;

        public SupportController(Kernel kernel)
        {
            _kernel = kernel;
        }

        [HttpGet("support")]
        public ActionResult<string> GetTechnicalSupport(string query)
        {
            // Invoke the kernel with a prompt and allow the AI to automatically invoke functions
            var settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };
            var response = await kernel.InvokePromptAsync(query, new(settings))
        }
    }
}
```

### Conclusion
In summary, it appears that the programming language of the future might closely resemble English. By providing a bridge between traditional coding practices and AI models, Semantic Kernel simplifies the process of implementing AI-driven solutions and opens new avenues for innovation. Through semantic understanding, developers are empowered to describe the capabilities of their code in a manner that AI can comprehend. This understanding enables seamless interactions between complex algorithms and existing software architectures, fostering a more intuitive development environment.

### Resources

1. [Microsoft Docs](https://learn.microsoft.com/en-us/semantic-kernel/overview/)
1. [GitHub Repository for Semantic Kernel](https://github.com/microsoft/semantic-kernel/)
1. [Semantic Kernel Starters](https://github.com/microsoft/semantic-kernel-starters)
1. [Tutorial: ChatGPT + Enterprise data with Semantic Kernel, OpenAI and Azure Cognitive Search](https://github.com/Azure-Samples/semantic-kernel-rag-chat)
1. [Using Semantic Kernel in C#](https://github.com/microsoft/semantic-kernel/blob/main/dotnet/README.md)
1. [Microsoft.SemanticKernel Namespace](https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel?view=semantic-kernel-dotnet)
