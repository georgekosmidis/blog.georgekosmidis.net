As a developer working with Azure services, you might find yourself needing to build and customize the Azure SDK to test a preview version of an Azure API that isn't yet available in the SDK. Sure, you could just use Postman, but where's the fun in that? :) Whether you're looking to make enhancements or support a new version that hasn't been officially released yet, having a clear set of instructions can save you a lot of time and hassle. In this post, we'll walk you through the process, from setting up your environment to pushing your custom changes to a forked repository.

## Prerequisites

Before we dive into the build process, make sure you have the following tools installed:

1. **Visual Studio 2022**: Ensure that the "Desktop development with C++" workload is included. While you can continue using VS Code for development, Visual Studio just needs to be installed, even if it's buried somewhere on your drive.
2. **Python 3.9.13**: If you're using a different version of Python, **pyenv** is a handy tool to manage multiple versions.
3. **Node 18.20.3**: Similarly, if you need to manage multiple Node versions, try **nvm**. 

## Step 1: Forking and Making Changes

If you need to fork the repository, make custom changes, and then build it, follow these steps:

1. **Clone the Repository**:
   ```sh
   git clone https://github.com/Azure/azure-sdk-for-js.git
   cd azure-sdk-for-js
   ```

2. **Check Out a Specific Tag** (Optional):
   ```sh
   git checkout tags/@azure/ai-form-recognizer_5.0.0 -b ai-form-recognizer_5.0.0
   ```

3. **Make Your Changes**: Open the code in your favorite editor and make the necessary modifications.

4. **Push Changes to Your Fork** (Optional):
   - Add a remote for your fork:
     ```sh
     git remote add fork https://github.com/<your-username>/azure-sdk-for-js.git
     ```
   - Push your changes:
     ```sh
     git push fork ai-form-recognizer_5.0.0
     ```
	 
## Step 2: Setting Up Your Environment

First, install the necessary software:

- **Visual Studio 2022**: Download and install from [https://visualstudio.microsoft.com/vs/](https://visualstudio.microsoft.com/vs/), ensuring you select the "Desktop development with C++" workload during installation. 
- **pyenv**: Follow the installation instructions from [pyenv's GitHub repository](https://github.com/pyenv/pyenv). Once installed, set it to use Python 3.9.13:
  ```sh
  pyenv install 3.9.13
  pyenv global 3.9.13
  ```
  <blockquote>If it is the first time you are using pyenv, it is advised to unistall all previous python versions.</blockquote>
  
- **nvm**: Install nvm from the [nvm GitHub repository](https://github.com/nvm-sh/nvm). Then install Node.js 18.20.3:
  ```sh
  nvm install 18.20.3
  nvm use 18.20.3
  ```

## Step 3: Installing and Using Rush

Next, you'll need to install Rush, a build orchestration tool used by Microsoft for managing large monorepos:

```sh
npm install -g @microsoft/rush
```

Once Rush is installed, navigate to your project directory and run the following commands to install dependencies and build the SDK:

```sh
rush install --purge
rush build
rush build -t @azure/ai-form-recognizer
```

This will build the specific package for the AI Form Recognizer.

## Step 4: Building the AI Form Recognizer Package

Navigate to the AI Form Recognizer package directory and pack the module:

```sh
cd .\sdk\formrecognizer\ai-form-recognizer
npm pack
```

After running this command, you should have a file named `azure-ai-form-recognizer-5.0.0.tgz`. Copy this file to a location of your choice for later use.
`

## That's it!

By following these steps, you'll be able to set up your development environment, build the SDK, make custom modifications, and push your changes to your forked repository. Happy coding!

Feel free to leave comments or ask questions if you run into any issues!