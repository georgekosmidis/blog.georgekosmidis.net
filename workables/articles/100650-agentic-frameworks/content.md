# Microsoft’s Agentic AI Frameworks

In the rapidly advancing world of AI, Microsoft is leading the charge in developing **Agentic AI** frameworks. These platforms enable developers to create autonomous, intelligent systems capable of executing complex tasks with minimal human intervention. The standout offerings from Microsoft - **AutoGen** and **Semantic Kernel** - are revolutionizing the possibilities of what AI systems can achieve.

> The two agentic framework teams [are working quickly and diligently to get to an alignment between the two Agentic Framewords](https://devblogs.microsoft.com/semantic-kernel/microsofts-agentic-ai-frameworks-autogen-and-semantic-kernel/), with the goal of making it available in early 2025 with version 0.4.

## What Are Agentic AI Frameworks?

Agentic AI frameworks are tools and libraries that simplify the creation of AI systems designed to act as "agents." These agents can perform tasks autonomously, coordinate with other agents, and interact with humans when needed. By using such frameworks, developers can design solutions that:

- Automate repetitive and intricate processes.
- Collaborate with other AI agents to manage multifaceted workflows.
- Provide real-time dynamic responses and adapt to new scenarios efficiently.

These frameworks are particularly valuable in a world increasingly reliant on AI-driven automation and interaction. 

### AutoGen

**AutoGen**, developed by Microsoft’s AI Frontiers Lab, focuses on creating distributed, event-driven systems where multiple AI agents collaborate to achieve complex goals. It is a versatile tool designed to simplify the development of multi-agent frameworks, enabling solutions for intricate workflows.

#### What Makes AutoGen Unique?

1. **Distributed Systems:** Supports the creation of agentic ecosystems where multiple agents work together, even across diverse environments.
2. **Event-Driven Workflows:** Allows agents to respond dynamically to events, making the framework highly adaptable to evolving requirements.
3. **Language Support:** Currently supports C# and Python, with plans for expanded language compatibility in the future.
4. **Human-AI Collaboration:** AutoGen’s agents can operate autonomously or include human oversight when necessary, making it suitable for fully automated or hybrid workflows.

#### Example Applications

- **Supply Chain Optimization:** Using AutoGen, companies can deploy a network of AI agents that coordinate to monitor inventory levels, predict demand, and automate order placements, ensuring just-in-time delivery.
- **Smart Home Management:** AutoGen can power a system where different agents manage lighting, temperature, and security autonomously, while interacting with the homeowner to adapt settings based on preferences.
- **Collaborative Problem Solving:** AutoGen enables the creation of a multi-agent system where individual agents tackle different parts of a complex problem, such as disaster response planning or multi-departmental project management.

#### Code Sample

Below is an example of creating a multi-agent system using AutoGen in Python. The script sets up a group chat simulation with three agents—a user proxy, a coder, and a product manager - using the `autogen` library. It configures each agent with specific roles and settings, including language model parameters for the coder and product manager, and code execution preferences for the user proxy. The group chat is managing a discussion aimed at finding a recent GPT-4 paper on arXiv and exploring its potential software applications.

```python
import autogen

# Define a list of configuration dictionaries for language models
config_list = [
    {
        "model": "your_model",                                # Specify the model name
        "api_version": "your_api_version",                    # API version to use
        "api_type": 'azure',                                  # Indicate that the API is hosted on Azure
        "api_rate_limit": 5,                                  # Set the rate limit for API calls per second
        "api_key": "YOUR_KEY",                                # Placeholder for the API key
        "base_url": "https://your_endpoint.openai.azure.com/",# Base URL for the API
        "price": [0, 0]                                       # Price for prompt and completion per 1k            
    }
]

# Create a dictionary for language model configuration, including a cache seed for reproducibility
llm_config = {"config_list": config_list, "cache_seed": 0}

# Initialize a UserProxyAgent representing a human admin
user_proxy = autogen.UserProxyAgent(
    name="User_proxy",                  # Name of the agent
    system_message="A human admin.",    # System message describing the agent
    code_execution_config={
        "last_n_messages": 4,           # Number of previous messages to consider
        "work_dir": ".autogen_chat",    # Working directory for code execution
        "use_docker": False,            # Whether to use Docker for code execution; set to True if available for safer execution
    },
    human_input_mode="TERMINATE",       # 'TERMINATE': human input is only requested when a termination condition is met. Also use 'NEVER' or 'ALWAYS'
)

# Initialize an AssistantAgent named "Coder" with the specified language model configuration
coder = autogen.AssistantAgent(
    name="Coder",
    llm_config=llm_config,
)

# Initialize another AssistantAgent named "Product_manager" with a system message 
pm = autogen.AssistantAgent(
    name="Product_manager",
    system_message="Creative in software product ideas.",
    llm_config=llm_config,
)

# Create a GroupChat instance with the user proxy, coder, and product manager agents
groupchat = autogen.GroupChat(
    agents=[user_proxy, coder, pm],  # List of agents participating in the chat
    messages=[],                     # Initial empty list of messages
    max_round=10                      # Maximum number of interaction rounds
)

# Initialize a GroupChatManager to manage the group chat, using the specified language model configuration
manager = autogen.GroupChatManager(
    groupchat=groupchat,
    llm_config=llm_config
)

# Start the chat by having the user proxy agent send an initial message
user_proxy.initiate_chat(
    manager,
    message="Find a latest paper about gpt-4 on arxiv and find its potential applications in software. When you are done, create a list of 3 potential products."
)

```

> This example was an adaptation of a script that can be found here: [https://microsoft.github.io/autogen/0.2/docs/notebooks/agentchat_groupchat/](https://microsoft.github.io/autogen/0.2/docs/notebooks/agentchat_groupchat/)


### Semantic Kernel

At the heart of Microsoft’s agentic AI strategy lies **Semantic Kernel**, an open-source SDK designed to seamlessly integrate large language models (LLMs) into various applications. With support for popular programming languages like C#, Python, and Java, Semantic Kernel equips developers with the flexibility to implement AI-driven features efficiently.

#### Key Features

1. **Language Model Integration:** Connect effortlessly to LLMs such as OpenAI's GPT models, enabling natural language understanding and generation.
2. **Agent Framework:** A recently introduced feature that allows developers to design single and multi-agent systems capable of collaborative or independent functionality.
3. **Event-Driven Architecture:** Supports asynchronous workflows, ensuring agents respond promptly to triggers and execute tasks in real time.
4. **Enterprise-Ready Tools:** Provides built-in capabilities for scalability, security, and deployment in enterprise environments.

#### Example Applications

- **Customer Support Bot:** Using Semantic Kernel, a company can create a chatbot that understands customer queries in natural language, retrieves relevant information, and responds intelligently - all while escalating complex issues to human agents when necessary.
- **Dynamic Content Generator:** Publishers can leverage Semantic Kernel to automatically generate personalized articles, marketing materials, or social media posts tailored to specific audiences, reducing workload and increasing engagement.
- **Autonomous Research Assistant:** Semantic Kernel can be used to develop an AI assistant that autonomously compiles reports, summarizes research, and even generates actionable insights from large datasets.

#### Code Sample

Here’s a simple example that creates a chatbot using Semantic Kernel for basic natural language interactions:

```python
import asyncio
from semantic_kernel.connectors.ai.open_ai import (
    AzureChatCompletion,
    AzureChatPromptExecutionSettings,
)
from semantic_kernel import Kernel
from semantic_kernel.contents import ChatHistory
from semantic_kernel.functions import KernelArguments

chat_completion_service = AzureChatCompletion(
    service_id="your_service_id",                       # Replace with your service identifier (just about anything will do!)
    deployment_name="your_model",                       # Replace with your Azure OpenAI deployment name
    api_key="YOUR_KEY",                                 # Replace with your Azure OpenAI API key
    endpoint="https://your_endpoint.openai.azure.com/"  # Replace with your Azure OpenAI endpoint
)

# Mind the "your_service_id" placeholder
request_settings = AzureChatPromptExecutionSettings(service_id="your_service_id") 

# This is the system message that gives the chatbot its personality.
system_message = "You are a chatbot with one goal: Find what people need!"

# Create a chat history object with the system message.
chat_history = ChatHistory(system_message=system_message)

kernel = Kernel()
chat_function = kernel.add_function(
    plugin_name="ChatBot",
    function_name="Chat",
    prompt="{{$chat_history}}{{$user_input}}",
    template_format="semantic-kernel",
)
kernel.add_service(chat_completion_service)


async def chat() -> bool:
    try:
        user_input = input("User:> ")
    except KeyboardInterrupt:
        print("\n\nExiting chat...")
        return False
    except EOFError:
        print("\n\nExiting chat...")
        return False
    if user_input == "exit":
        print("\n\nExiting chat...")
        return False

    # Get the chat message content from the chat completion service.
    kernel_arguments = KernelArguments(
        settings=request_settings,
        # Use keyword arguments to pass the chat history and user input to the kernel function.
        chat_history=chat_history,
        user_input=user_input,
    )

    answer = await kernel.invoke(plugin_name="ChatBot", function_name="Chat", arguments=kernel_arguments)
    # Alternatively, you can invoke the function directly with the kernel as an argument:
    # answer = await chat_function.invoke(kernel, kernel_arguments)
    if answer:
        print(f"Mosscap:> {answer}")
        # Since the user_input is rendered by the template, it is not yet part of the chat history, so we add it here.
        chat_history.add_user_message(user_input)
        # Add the chat message to the chat history to keep track of the conversation.
        chat_history.add_message(answer.value[0])

    return True


async def main() -> None:
    chatting = True
    while chatting:
        chatting = await chat()

if __name__ == "__main__":
    asyncio.run(main())
```

> This was an adaptation of the chat completion services sample from the Semantic Kernel repository. The original sample can be found here: [https://github.com/microsoft/semantic-kernel/blob/main/python/samples/concepts/setup/chat_completion_services.py](https://github.com/microsoft/semantic-kernel/blob/main/python/samples/concepts/setup/chat_completion_services.py)


## Integration and Future Alignment

In November 2024, Microsoft announced a significant milestone: the integration of AutoGen’s multi-agent runtime with Semantic Kernel. This effort aims to offer developers a unified framework for experimenting with advanced agentic patterns and transitioning smoothly to production-ready environments. The integration is expected to complete by early 2025, bringing changes to the AutoGen API. Additionally, Microsoft introduced AG2, a branch of AutoGen maintained by its original creators, which will preserve existing features and remain unaffected by the forthcoming changes.

### Versions Overview

- **AutoGen v0.2:** The current version.
- **AutoGen v0.3:** This branch will be rebranded as [AG2](https://ag2.ai/), continuing as an independent project.
- **AutoGen v0.4:** Currently in [preview](https://microsoft.github.io/autogen), this represents the future of AutoGen, aligned with Semantic Kernel.

Until the integration is finalized, developers are advised to:
- Use **Semantic Kernel** for production environments.
- Leverage **AutoGen** for experimentation.
- Consider the **AG2** path to avoid the migration overhead from AutoGen v0.2 to v0.4.

### Why This Matters

- **Seamless Transition:** Developers can prototype ideas using AutoGen’s flexible architecture and scale them efficiently with Semantic Kernel.  
- **Consistency Across Frameworks:** Unified APIs and tools simplify development, reducing learning curves and speeding up workflows.  
- **Expanded Possibilities:** From automating business operations to creating interactive gaming agents, the integration unlocks a wide range of applications.

This alignment marks a transformative step in unifying the agentic AI ecosystem, significantly enhancing its utility for developers and enterprises. Below is an example showcasing the combined capabilities of Semantic Kernel and AutoGen. 



## Beyond Semantic Kernel and AutoGen

Microsoft’s commitment to agentic AI extends beyond Semantic Kernel and AutoGen. Additional frameworks and tools illustrate the company’s dedication to pushing the boundaries of AI innovation:

- **TinyTroupe:** TinyTroupe is an experimental Python library that allows the simulation of people with specific personalities, interests, and goals. These artificial agents - `TinyPersons` - can listen to us and one another, reply back, and go about their lives in simulated `TinyWorld` environments. Read more here: [https://github.com/microsoft/TinyTroupe](https://github.com/microsoft/TinyTroupe)
- **Magentic-One:**  Build on AutoGen, it's a sophisticated framework for orchestrating specialized agents to tackle complex, domain-specific challenges. For example, Magentic-One can be used to manage a healthcare system where agents specialize in tasks like patient triage, appointment scheduling, and resource allocation. Each agent operates independently, but the framework ensures seamless coordination, enabling a comprehensive solution. 

These complementary tools expand the scope of agentic AI, offering new avenues for innovation in various industries.


## Ready to Build the Future?

With frameworks like Semantic Kernel, AutoGen, and more, Microsoft is opening the door to a new world of possibilities. Start exploring these innovative tools today and craft AI systems that think, adapt, and collaborate seamlessly. The future of intelligent technology is here - and it’s ready for you to shape!

## Relevant Links

- [Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/)
- [AutoGen](https://microsoft.github.io/autogen)
- [AG2](https://ag2.ai/)
- [Magentic-One Details](https://github.com/microsoft/magentic-one)
- [TinyTroupe Repository](https://github.com/microsoft/tinytroupe)