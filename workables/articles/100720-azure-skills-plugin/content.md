Microsoft recently released the [Azure Skills Plugin](https://aka.ms/azure-skills) - a package that gives AI coding agents like GitHub Copilot and Claude Code genuine Azure expertise instead of generic cloud advice. This article distills all announcments into a single cookbook you can bookmark and reference whenever you need it.

> The Azure Skills Plugin bundles 21 curated skills, the Azure MCP Server (~200 tools across 40+ services), and the Foundry MCP Server in a single install. Skills provide the workflow logic; MCP provides the execution layer.

## What the Plugin Actually Is

The plugin is **not** a prompt pack or a folder of clever one-liners. It ships three layers:

| Layer | What It Does | Scale |
| ----- | ------------ | ----- |
| **Azure Skills** | Reusable workflows, decision trees, and guardrails that teach the agent *how* Azure work gets done | 24 skill files |
| **Azure MCP Server** | Structured tools for listing resources, checking prices, querying logs, provisioning infra, and driving deployments | ~200 tools, 40+ services |
| **Foundry MCP Server** | Model deployment, agent management, and model catalog workflows for AI-powered apps | AI Foundry integration |

Skills are plain-text markdown files (`SKILL.md`) that the agent reads on demand. They do not run code themselves - they instruct the agent on when, how, and in what order to call MCP tools. The result is that a single prompt like "Deploy my Python Flask API to Azure" triggers a coordinated chain of `azure-prepare`, `azure-validate`, and `azure-deploy` instead of producing a generic tutorial.

## Prerequisites

Before installing, make sure you have:

- **Node.js 18+** on your PATH (MCP servers run via `npx`)
- **Azure CLI (`az`)** installed and authenticated (`az login`)
- **Azure Developer CLI (`azd`)** installed and authenticated (`azd auth login`) for deployment workflows
- **An Azure account** (free tier works for exploring, real subscription for deployments)
- **Git CLI** (required for the VS Code extension)

## Installation

### GitHub Copilot CLI

```bash
# Add the marketplace source (first time only)
/plugin marketplace add microsoft/azure-skills

# Install the plugin
/plugin install azure@azure-skills

# IMPORTANT: reload MCP servers after install
/mcp reload
```

Verify with `/mcp status` - you should see `azure` and `context7` servers listed and running.

### VS Code

Install the **Azure MCP** extension from the VS Code Marketplace (Extension ID: `ms-azuretools.vscode-azure-mcp-server`). It auto-installs a companion extension containing the Azure skills. After install:

1. Open Copilot Chat (`Ctrl+Shift+I` / `Cmd+Shift+I`)
2. Switch to **Agent mode** (not Ask or Edit mode)
3. Command Palette (`Ctrl+Shift+P`) > search "MCP" > verify servers are running

### Claude Code

```bash
# Add the marketplace source (first time only)
claude plugin marketplace add microsoft/azure-skills

# Install the plugin
claude plugin install azure@azure-skills
```

### Smoke Test (All Hosts)

Try these two prompts to confirm everything works:

```
"List my Azure resource groups"
```

If you get live results from your subscription, MCP tools are working.

```
"What Azure services would I need to deploy a Python web API?"
```

If the agent references `azure-prepare` and gives structured guidance (not generic suggestions), skills are loaded.

## Skills Quick Reference

Here is the complete list of all 21 skills, what they do, and a sample prompt for each.

### Infrastructure Planning

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-enterprise-infra-planner` | Architects enterprise-scale infrastructure - landing zones, hub-spoke networks, multi-region DR topologies, subscription-scope Bicep deployments | `"Design a hub-spoke network with a firewall and private endpoints for my production workloads"` |

> This skill is for infrastructure-first planning (landing zones, network topologies, multi-region designs). For app-centric workflows where you're deploying an application, use `azure-prepare` instead.

### Deploy Lifecycle

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-prepare` | Analyzes your project, recommends services, generates Bicep/Terraform, Dockerfile, `azure.yaml` | `"Prepare my Node.js Express API for Azure Container Apps"` |
| `azure-validate` | Pre-flight checks on infra code, auth, permissions | `"Validate my Azure configuration before I deploy"` |
| `azure-deploy` | Orchestrates deployment via `azd`, provisions infra, pushes images, returns live URL | `"Deploy this project to Azure"` |

> You rarely need to call these individually. A single prompt like "Create and deploy this Python Flask API to Azure Container Apps" chains all three automatically.

### Cost and Quotas

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-cost-optimization` | Finds orphaned resources, oversized VMs, wrong storage tiers, idle dev environments | `"Analyze my Azure spending and find savings"` |
| `azure-quotas` | Shows current usage vs. limits for compute in any region | `"How many vCPUs do I have available in East US?"` |

### Resource Discovery

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-resource-lookup` | Queries Azure Resource Graph to find and filter resources | `"Find resources tagged with environment=staging"` |
| `azure-resource-visualizer` | Generates Mermaid architecture diagrams from a resource group | `"Generate an architecture diagram of my resource group 'prod-rg'"` |

### Compute

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-compute` | Recommends VM sizes with real-time pricing from Azure Retail Prices API | `"What VM size should I use for a Python ML training job with 64GB RAM and a GPU?"` |
| `azure-kubernetes` | It covers AKS cluster planning, SKU selection, networking, and Day-0 configuration decisions | `"Create a production-ready AKS cluster with private API server and Azure CNI Overlay"` |

### Diagnostics

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-diagnostics` | Runs KQL queries against Log Analytics, correlates events, identifies root cause | `"My container app is returning 503 errors - what's wrong?"` |

### Security and Identity

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-rbac` | Recommends least-privilege roles, generates `az role assignment` commands and Bicep | `"What's the least-privilege role for reading blobs from a storage account?"` |
| `azure-compliance` | Audits resources against Azure best practices, checks Key Vault expiration | `"Run a compliance scan on my subscription"` |
| `entra-app-registration` | Full app registration flow: create, configure redirect URIs, set API permissions, generate MSAL code | `"Create a new Entra ID app registration for my web API"` |

### AI and Foundry

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-ai` | SDK setup and working code for Azure AI Search, Speech, OpenAI, Document Intelligence | `"Set up Azure AI Search with vector search for my documents"` |
| `microsoft-foundry` | Deploys models, creates agents, runs evaluations, manages Foundry projects | `"Deploy a GPT-4o model through Microsoft Foundry"` |
| `azure-aigateway` | Configures Azure API Management with semantic caching, token limits, content safety | `"Set up Azure API Management as an AI Gateway in front of my Azure OpenAI models"` |

### Migration

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-cloud-migrate` | Maps AWS/GCP services to Azure equivalents, converts infra code, generates migration assessments | `"Assess my AWS Lambda functions for migration to Azure Functions"` |
| `azure-upgrade` | That section is currently about cross-cloud migration; this skill handles intra-Azure upgrades between plans, tiers, and SKUs (e.g. Consumption to Flex Consumption, App Service to Container Apps) | `"Upgrade my function app from Consumption to Flex Consumption"` |

> `azure-cloud-migrate` handles migration from other cloud providers (AWS, GCP) to Azure. `azure-upgrade` handles upgrades within Azure - changing plans, tiers, or SKUs (e.g. moving from Consumption to Flex Consumption, or App Service to Container Apps).

### Observability and Data

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `appinsights-instrumentation` | Generates SDK setup and telemetry patterns for Application Insights | `"Add Application Insights telemetry to my Express.js API"` |
| `azure-kusto` | Generates and runs KQL queries against Azure Data Explorer | `"Write a KQL query to find the top 10 slowest API requests in the last hour"` |

### Storage and Messaging

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-storage` | Working code for Blob, File, Queue, Table operations; lifecycle policies and access tiers | `"Set up lifecycle management to move old blobs to cool storage"` |
| `azure-messaging` | Troubleshoots Event Hubs/Service Bus SDK issues; generates producer/consumer code | `"Set up a Service Bus queue with dead-letter handling in Python"` |

### Copilot SDK

| Skill | Purpose | Try This |
| ----- | ------- | -------- |
| `azure-hosted-copilot-sdk` | Scaffolds a Copilot SDK app with session management, model config, and Azure deployment | `"Build a GitHub Copilot SDK app and deploy it to Azure"` |

## Prompting Tips

### Be Specific About Your Stack

| Vague | Better |
| ----- | ------ |
| "Deploy my app" | "Deploy my Python FastAPI app to Azure Container Apps" |
| "Fix my Azure errors" | "My container app `checkout-api` is returning 503s since 6 AM" |
| "Set up a database" | "Create a PostgreSQL Flexible Server with Entra ID auth for my Node.js API" |

### Name Your Resources

Include resource names so the agent can query them directly without asking follow-up questions:

```
"Show me errors from the container app 'payments-api' in the last 4 hours"
```

### Chain Prompts for Complex Workflows

You can build up a full workflow incrementally:

1. "What Azure services would I need for this project?"
2. "Prepare it for Azure Container Apps with a PostgreSQL backend"
3. "Validate the configuration"
4. "Deploy it"
5. "Set up Application Insights monitoring"
6. "Run a cost optimization scan"

### Ask About Trade-offs

Skills can explain Azure decisions, not just execute them:

```
"Should I use Container Apps or App Service for this API?"
"What's the trade-off between Bicep and Terraform for this project?"
```

## Troubleshooting

### Skills Not Activating

- **Copilot CLI:** Run `/plugin list` to confirm the plugin is installed. Check budget with `/context`.
- **VS Code:** Confirm you are in **Agent mode**. Reload window (`Ctrl+Shift+P` > "Reload Window").
- **Claude Code:** Run `/skills` to confirm. Check budget with `/context`.

### MCP Tools Not Showing

- **Copilot CLI:** Run `/mcp reload` (required after install), then `/mcp show azure`.
- **VS Code:** Check `.mcp.json` in workspace root. Try Command Palette > "MCP: Restart Servers".
- **All hosts:** Confirm `node --version` returns 18+.

### Auth Errors

- Refresh with `az login` and `azd auth login`.
- Set your subscription: `az account set --subscription <your-sub-id>`.

### Skill Budget Limits

Each host limits how much skill content can be loaded per request:

| Host | Budget | Override |
| ---- | ------ | -------- |
| **Copilot CLI** | 15,000 tokens | Environment variable |
| **Claude Code** | 2% of context window (floor: 16,000 chars) | `SLASH_COMMAND_TOOL_CHAR_BUDGET` env var |

If you have many plugins/MCP servers loaded, some skills may be silently excluded. Disable unused skills or reduce total tool count to free up space.

## What Gets Installed

The plugin payload lands under `.copilot/installed-plugins/azure-skills/azure/` and contains:

- `skills/` - 21 `SKILL.md` files
- `.mcp.json` - MCP server definitions (the `azure` server and `context7` for up-to-date docs)

The `.mcp.json` configures two servers:

```json
{
  "servers": {
    "azure": {
      "command": "npx",
      "args": ["-y", "@azure/mcp@latest", "server", "start"]
    },
    "context7": {
      "command": "npx",
      "args": ["-y", "@upstash/context7-mcp@latest"]
    }
  }
}
```

If you already had the Azure MCP Server installed separately, the plugin simply adds the skills layer on top. Your existing MCP tools continue to work.

## Links

- **Install the plugin:** [aka.ms/azure-plugin](https://aka.ms/azure-plugin)
- **Plugin repo:** [aka.ms/azure-skills](https://aka.ms/azure-skills)
- **Troubleshooting guide:** [GitHub - Troubleshooting.md](https://github.com/microsoft/azure-skills/blob/main/Troubleshooting.md)
- **Announcement series:** [Part 1 - Announcement](https://devblogs.microsoft.com/all-things-azure/announcing-the-azure-skills-plugin/) | [Part 2 - Installation](https://devblogs.microsoft.com/all-things-azure/azure-skills-plugin-lets-get-started/) | [Part 3 - Prompt Cookbook](https://devblogs.microsoft.com/all-things-azure/building-with-azure-skills/)