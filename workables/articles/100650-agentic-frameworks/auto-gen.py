# Import the autogen library, which provides tools for creating automated agents and group chats
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

# Initialize another AssistantAgent named "Product_manager" with a system message indicating creativity in software product ideas
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
