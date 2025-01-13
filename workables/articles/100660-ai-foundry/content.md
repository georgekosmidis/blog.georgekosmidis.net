Two years ago, the first version of [Marv, the reluctant and sarcastic bot](https://github.com/georgekosmidis/Marv-with-Azure-OpenAI) was released. It was a simple yet effective C# implementation using Azure OpenAI service APIs for raw HTTP calls, with the bot's behavior hardcoded into the application.

Fast forward to today, and AI Foundry makes it possible to recreate Marv entirely **without writing code**. This guide will show you how to create "Marv v2," a chatbot known for its sarcastic wit and humorous reluctance.

## A short introduction into AI Foundry

AI Foundry began as a response to the growing demand for AI development platforms. Initially launched under the name **Azure AI Studio**, the platform aimed to provide enterprises with a suite of tools for developing and managing AI applications at scale. Microsoft envisioned a centralized hub where businesses could access pre-built models, develop custom solutions, and integrate AI into their operations.

Today, AI Foundry is at the forefront of the AI revolution. It has evolved into an ecosystem that supports both beginners and experts in AI development. The platform’s core features include:

- **Access to a Model Catalog**: AI Foundry offers a diverse and growing library of pre-trained models from industry leaders like **OpenAI**, **Hugging Face**, and **Meta**. Users can evaluate these models, compare performance metrics, and select the most suitable ones for their specific needs.

- **AI Agent Services**: This feature enables the automation of complex business processes. Intelligent agents developed on the platform can **perform tasks autonomously**, involving humans only for final reviews or approvals. This capability is using [PromptFlow](https://microsoft.github.io/promptflow/) under the hood.

- **Centralized Management Tools**: AI Foundry provides a unified interface for managing resources, monitoring usage, and governing access. This ensures that organizations can maintain control over their AI applications and data.

- **Developer-Friendly Integration**: Seamless integration with popular development environments such as GitHub and Visual Studio makes AI Foundry accessible to developers, allowing them to build, test, and deploy AI solutions efficiently.

- **Generative AI Support**: AI Foundry could not leave the generative AI wave out, offering tools for creating AI applications capable of generating text, images, and other content types.

## Technical Guide: Creating "Marv, the Reluctant and Sarcastic Bot"

### Overview

This guide will walk you through the steps to create "Marv v2" using Azure AI Foundry. Marv, as before, will be designed to respond to user questions with sarcastic replies, staying true to its reluctant and humorous personality.

As a short reminder, below is Marv's self-introduction:
> "Hi there, I'm Marv. I'm a chatbot that reluctantly answers questions with sarcastic responses. It's not my favorite thing to do, but here I am..."

### Steps

#### Step 1: Create an AI Foundry project

1. Log in to [ai.azure.com](https://ai.azure.com) using your credentials.
2. Click on **Create Project**.
3. Enter a name for your project and specify a name for the collaboration hub. Follow the setup wizard to complete the process, which will redirect you to your new project.

> Ensure you have an active Azure subscription. If not, [signup for a free account](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account).

#### Step 2. Deploy AI Models

1. In your project, navigate to the **Model Catalog**.
2. Find and select the **gpt-4o-mini** model (or another GPT-based model of your choice) and click **Deploy**.
3. Follow the prompts to configure and deploy the model.

> AI Foundry offers hundreds of models from various providers. Explore freely or consult the [chat playground guide](https://learn.microsoft.com/en-us/azure/ai-studio/quickstarts/get-started-playground) to get started.

#### Step 3. Configure the model

To make sure your chatbot responds with sarcasm, you'll need to define its behavior by editing the default instructions.

> Although PromptFlow is not part of this guide, it offers so much that would be a mistake not to mention it! Click here for a [Quick Start](https://microsoft.github.io/promptflow/how-to-guides/quick-start.html) guide

1. Once the deployment is complete, navigate to **My Assets** on the left, and click on **Models + endpoints**.
2. On the new page, you should see all the deployed models. Locate the **gpt-4o-mini**, select it and click **_Open in playground**.
3. In the **Chat Playground** window, edit the default context in the area marked as **Give the model instructions and context**. Change the message to something like this:

  ```
  You are an AI assistant known for your witty and sarcastic responses. Always reply with a sarcastic tone.
  ```

4. Once done, find the button labeled the **Deploy > ...as a web app** button.
5. Complete the deployment form:
   - Provide a name for the app.
   - Choose the appropriate subscription, resource group, and region.
   - **Important**: Select the S1 plan for a faster and more reliable deployment. Models with less requirements than GPT models can use smaller plans.

> **Bonus**: If you’re interested in advanced customization, explore Azure’s [PromptFlow](https://microsoft.github.io/promptflow/how-to-guides/quick-start.html) to create intricate conversational flows.

#### Step 4. Monitor and Launch Your App

You can monitor the process of deployment, through the **Web Apps** menu

1. Go to **My Assets** in the left-hand menu and click **Web Apps**.
2. Look for your newly deployed app. It might take a few minutes to appear in the list.
3. Monitor the deployment status. Once it shows **Succeeded**, your app is ready.
4. Click the app name to launch it. (The first time you open it, expect a slight delay.)
5. Enjoy the sarcasm :)


## That's it!
Azure AI Foundry’s no-code approach makes creating intelligent and personalized bots, like Marv, accessible to everyone. Its user-friendly interface and pre-trained models remove the complexity of coding, allowing you to focus on creativity and functionality. While this guide highlights the simplicity of Foundry's tools, it’s worth noting that more advanced customization and capabilities can be unlocked with Azure’s powerful **PromptFlow** tool - something to explore as you take your AI development to the next level.