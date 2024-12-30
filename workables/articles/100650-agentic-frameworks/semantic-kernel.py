# This is an adaptation of the chat completion services sample from the Semantic Kernel repository.
# The original sample can be found here: https://github.com/microsoft/semantic-kernel/blob/main/python/samples/concepts/setup/chat_completion_services.py

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