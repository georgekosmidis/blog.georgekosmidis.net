In this article, we’ll explore how to automate the deployment of workflows in Azure Logic Apps using the Azure CLI. We’ll cover a scenario where multiple workflows are organized in subfolders, and we’ll use a simple Bash script to zip each workflow and deploy it programmatically.

## Introduction

Azure Logic Apps is a cloud service that helps you automate workflows that integrate apps, data, services, and systems. When working with the Standard Plan, you may need to deploy multiple workflows in a structured and automated way. This guide will show you how to use the Azure CLI to deploy workflows in bulk by zipping each one and pushing it to your Azure environment.

### Pre-requisites

Before we dive into the solution, ensure you have the following:

- An active Azure subscription.
- The Azure CLI installed on your local machine for testing.
- A service connection in Azure DevOps, if you're using it.


## Step-by-Step Guide

### 1. Organizing Your Logic App Workflows

Your Logic App Workflows should be structured in a folder. Each workflow should have its own subfolder under a main directory (e.g., `./mylogicapp/workflows/sample_workflow`). Inside each subfolder, the Logic App's workflow files should be present.

Example Directory Structure:
```bash
mylogicapp/
  └─ workflows/
      ├─ workflow1/
      ├─ workflow2/
      └─ workflow3/
```

### 2. Writing the Azure CLI Deployment Script

We'll use a Bash script to:

1. Zip each Logic App (workflow).
2. Deploy the zip file to Azure using the Azure CLI command `az logicapp deployment source config-zip`.

Here’s the script that automates this process:

```bash
# Set the directory where your logic apps are located
LOGIC_APP_DIR="./mylogicapp/workflows/"

# Loop through each subfolder
for folder in "$LOGIC_APP_DIR"/*; do
  if [ -d "$folder" ]; then
    # Create a zip file for each subfolder (Logic App)
    folder_name=$(basename "$folder")
    zip_file="/tmp/${folder_name}.zip"
    
    zip -r "$zip_file" "$folder"
    
    # Deploy the Logic App to Azure
    az logicapp deployment source config-zip \
      --resource-group "${RG}" \
      --name "${LOGIC_APP_NAME}" \
      --src "$zip_file"

    echo "Deployed $folder_name logic app using $zip_file"
  fi
done
```

### 3. Explanation of Key Commands

#### Azure CLI Command

```bash
az logicapp deployment source config-zip
```

This command allows you to deploy a Logic App by specifying a zip file that contains the workflow definition and related resources. The key parameters are:

- `--resource-group`: Specifies the resource group where the Logic App is deployed.
- `--name`: The name of the Logic App instance.
- `--src`: The path to the zip file that contains the Logic App’s workflow definition.

#### Bash Script Breakdown

- Folder Traversal: The script loops through each subfolder in the directory.
- Zipping the Folder: Each subfolder is zipped into a file with the same name.
- Deploying to Azure: The `az logicapp deployment` command pushes the zip file to Azure, deploying the workflow.

### 4. Running the Script in an Azure DevOps Pipeline

If you're working with Azure DevOps, you can integrate this script into a YAML pipeline. Here's an example task definition to run this script:

```yaml
- task: AzureCLI@2
  displayName: 'Deploy Logic Apps Workflows'
  inputs:
    azureSubscription: '$(ArmServiceConnectionName)'
    scriptType: 'bash'
    scriptLocation: 'inlineScript'
    inlineScript: |
      LOGIC_APP_DIR="./backend/analytics/de/logicapps"
      for folder in "$LOGIC_APP_DIR"/*; do
        if [ -d "$folder" ]; then
          folder_name=$(basename "$folder")
          zip_file="/tmp/${folder_name}.zip"
          zip -r "$zip_file" "$folder"
          
          az logicapp deployment source config-zip \
            --resource-group "${RG}" \
            --name "${LOGIC_APP_NAME}" \
            --src "$zip_file"
          
          echo "Deployed $folder_name logic app using $zip_file"
        fi
      done
```

### 5. Troubleshooting and Tips

- Permissions: Make sure your service principal or Azure DevOps service connection has sufficient permissions to deploy Logic Apps in the target resource group.
- Error Handling: You may want to add error handling in the script to handle scenarios where the deployment fails.
- Naming Conventions: Ensure that the names of your Logic Apps and their subfolders are consistent and unique.


## Further Reading

- [Official Azure CLI Documentation](https://docs.microsoft.com/en-us/cli/azure/)
- [Azure Logic Apps Documentation](https://docs.microsoft.com/en-us/azure/logic-apps/)