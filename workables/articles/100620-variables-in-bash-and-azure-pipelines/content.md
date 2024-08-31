# Contents

...

# Intro

In this article, we'll explore the basics of variables in Bash, focusing on the role of symbols like $, (, [, and {. Each of these symbols has a specific function in Bash scripting, from referencing variables to grouping commands and conditions. Understanding these will not only enhance your scripting skills but will also give you a better grasp of how similar concepts are applied in Azure Pipelines.


# Part 1. Variables in Bash

Bash scripting is a powerful tool for automating tasks in Unix-based systems, and understanding how variables work is fundamental to making the most of it. In this section, we’ll break down how variables are defined and used in Bash, focusing on the key symbols: `$`, `()`, `[]`, and `{}`. Each of these plays a distinct role in how you interact with and manipulate variables within your scripts.

### a. The `$` Sign

The `$` sign is used to reference variables in Bash. When you define a variable, you simply assign a value to a name, like this:

```bash
MY_VAR="Hello, World!"
```

To access the value stored in `MY_VAR`, you prefix it with a `$`:

```bash
echo $MY_VAR
```

This would output `Hello, World!` to the terminal. 

> The `$` sign is also used for special variables that hold particular data, such as `$0` for the script name, `$1`, `$2`, etc., for positional parameters passed to the script, and `$#` for the number of arguments.


## b. Parentheses `(...)`

### `a=$(command)`: Parentheses for variables

Parentheses in Bash are primarily used for command substitution and grouping commands. When you use `$(...)`, the command inside the parentheses is executed, and its output is returned and can be stored in a variable:

```bash
DATE=$(date)
echo $DATE
```

This script assigns the current date and time to the variable `DATE` and then echoes it. This approach is useful for capturing the output of commands directly into variables.

### `a=(a1 a2 a3)`: Parentheses for arrays

Parentheses are also used for defining arrays:

```bash
my_array=(apple orange banana)
echo ${my_array[0]}
```

In this example, `my_array` is an array containing three elements, and `apple` (the first element) is accessed using `${my_array[0]}`.

### `a=((1 < 2))` Arithmetic Expressions

Bash also offers double parentheses which is used to evaluate arithmetic expressions. Inside (( ... )), you can perform calculations like addition, subtraction, multiplication, division, and modulus. Here's a basic example:


```bash
a=5
b=3
result=$((a + b))
echo $result
# Outputs: 8
```

Double parentheses can also be used for conditional expressions that involve arithmetic comparisons. For example:
```bash
x=10
if (( x > 5 )); then
  echo "x is greater than 5"
fi

```

## c. Square Brackets `[]`

### `if [ a == b ]`: Simple Conditions

Square brackets in Bash are used in conditional expressions, particularly within `if` statements. They evaluate expressions and return a true or false value:

```bash
MY_VAR="Hello, World!"
if [ $MY_VAR == "Hello, World!" ]; then
  echo "The greeting is correct."
fi
# Outputs: "The greeting is correct."
```

This condition checks if `MY_VAR` equals "Hello, World!" and executes the block of code if the condition is true. 

### `if [[ a =~ ".*b" ]]`: Complex Conditions with 

Bash also offers double square brackets `[[ ]]`, which are more powerful and flexible, allowing for more complex conditions without the need for escaping certain characters, e.g. the use of Regular Expressions:s

```bash
MY_VAR="Hello, World!"
if [[ $MY_VAR =~ ".*World!" ]]; then
  echo "The greeting is correct."
fi
# Outputs: "The greeting is correct."
```

## d. Curly Braces `{}`

### `a=${VAR}s`: Variable Expansion

Curly braces are used in Bash for variable expansion and grouping expressions. They allow you to manipulate variables more precisely:

```bash
ANIMAL="bear"
echo "These are ${ANIMAL}s!"
# Output: These are bears!
```

This script outputs `These are bears!`. The braces make it clear where the variable name ends, allowing you to append text directly to the variable.

### `for {1..5}`: Loops and brace expansion 

Curly braces are also useful in loops and brace expansion:

```bash
for i in {1..5}; do
  echo "Number $i"
done
```

This loop iterates from 1 to 5, printing each number. Brace expansion can be a powerful tool for generating sequences or lists.

# Part 2. Variables in Azure Pipelines Tasks

## a. Using Variables in Azure Pipelines

In Azure Pipelines, variables are essential for customizing and controlling your CI/CD workflows. They allow you to store and reuse values across different stages, jobs, and tasks within a pipeline. Azure Pipelines comes with a set of predefined variables that provide useful information about the pipeline run, such as `Build.BuildId`, `System.JobName`, and `Agent.WorkFolder`. These predefined variables are available automatically and can be referenced to retrieve key details about the environment and execution context.

You can also define your own variables in Azure Pipelines. These custom variables can be defined at different levels: pipeline, stage, job, or task. They are defined in your pipeline YAML file using the `variables` keyword. Here’s a basic example of defining and using a variable in an Azure Pipelines YAML file:

```yaml
variables:
  myVariable: 'Hello, World!'

steps:
- script: echo $(myVariable)
  displayName: 'Display myVariable'
```

In this example, `myVariable` is defined at the pipeline level and can be referenced using the `$(variableName)` syntax. The `script` task then uses `$(myVariable)` to echo the value of the variable. This syntax is how you generally reference variables throughout Azure Pipelines, allowing you to inject dynamic content into your pipeline scripts and tasks.

## b. Mapping Bash Concepts to Azure Pipelines

When you move from Bash scripting to Azure Pipelines, it’s important to understand how the symbols `$`, `()`, `[]`, and `{}` translate to variable handling in the pipeline context.

- **`$` Sign**: Just like in Bash, the `$` sign in Azure Pipelines is used to reference variables. In Bash, `$` is followed by the variable name to access its value, e.g., `$MY_VAR`. In Azure Pipelines, you use `$(variableName)` to achieve a similar effect. However, unlike Bash, **the parentheses `()` are part of the syntax in Azure Pipelines**.

- **Parentheses `()`**: In Bash, `()` are often used for command substitution or to group commands in subshells. In Azure Pipelines, parentheses are incorporated into the syntax for variable references, i.e., `$(variableName)`. This helps distinguish variable references from other text within the YAML file, making it clear where a variable is being substituted.

- **Square Brackets `[]`**: In Bash, `[]` are typically used in conditional expressions, such as `[ $MY_VAR -eq 1 ]`. In Azure Pipelines, while you don't use square brackets directly for conditions in the same way, they play a role in YAML syntax, particularly for lists and conditions within pipeline definitions. For example, you might see them used in condition expressions like this:

  ```yaml
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  ```

  Here, square brackets are used to reference variables in conditions, particularly when the variable name includes special characters or when accessing nested variables.

- **Curly Braces `{}`**: In Bash, curly braces are used for variable expansion and grouping, like `${MY_VAR}`. In Azure Pipelines, curly braces are less common but might appear in expressions or template syntax. For instance, when defining expressions for conditions or when using template expressions, curly braces can be used:

  ```yaml
  steps:
  - script: echo "Value of myVar is ${{ variables.myVariable }}"
    displayName: 'Display myVariable using curly braces'
  ```

  Here, `${{ }}` is used to evaluate expressions and access variables in a more complex way than the simple `$(variableName)` syntax.

### Examples of Using These Concepts in Pipeline Tasks

Consider the following examples that showcase how these concepts are utilized in pipeline tasks:

1. **Command-line Task with Variables**:
   ```yaml
   variables:
     myNumber: 10

   steps:
   - script: echo "The value is $(myNumber)"
     displayName: 'Echo myNumber'
   ```

   In this example, the variable `$(myNumber)` is referenced within a script task, similar to how you'd use `$myNumber` in Bash.

2. **Using Variables in Conditions**:
   ```yaml
   steps:
   - script: echo "This will run only if the branch is main"
     condition: eq(variables['Build.SourceBranch'], 'refs/heads/main')
     displayName: 'Conditional Task'
   ```

   Here, square brackets are used within the `condition` to safely access the `Build.SourceBranch` variable.

# Part 3. Combining Bash and Azure Pipelines

## a. Running Bash Scripts in Azure Pipelines

Running Bash scripts within Azure Pipelines is a common practice, especially when you need to execute complex logic or leverage existing scripts. Azure Pipelines offers built-in support for Bash, allowing you to run scripts directly in your pipeline tasks.

To run a Bash script in an Azure Pipeline, you simply define a `bash` task within your pipeline YAML file:

```yaml
- task: Bash@3
  inputs:
    filePath: 'scripts/myscript.sh'
```

You can also inline your Bash script directly within the YAML file:

```yaml
- script: |
    echo "Running a script"
    ./myscript.sh
  displayName: 'Run myscript.sh'
```

When working with variables, you can pass Azure Pipeline variables to your Bash script as environment variables or command-line arguments. For example:

```yaml
- script: |
    echo "Running my script with arguments"
    ./myscript.sh $(PipelineVar)
  displayName: 'Run myscript.sh with PipelineVar'
```

In this example, `$(PipelineVar)` is an Azure Pipeline variable that gets passed as an argument to the Bash script. Inside the `myscript.sh` script, you can access it using `$1` or similar positional parameters.

Alternatively, you can pass variables as environment variables:

```yaml
- script: |
    echo "Running my script with environment variable"
    export MY_ENV_VAR=$(PipelineVar)
    ./myscript.sh
  displayName: 'Run myscript.sh with environment variable'
```

## b. Variable Interchange Between Bash and Azure Pipelines

One of the powerful features of Azure Pipelines is the ability to set pipeline variables from within your Bash scripts. This allows you to output values from a script and use them in subsequent tasks or jobs.

To set a variable in Azure Pipelines from a Bash script, you use the special `##vso` logging command:

```bash
echo "##vso[task.setvariable variable=myVar]value"
```

In this command, `myVar` is the name of the pipeline variable, and `value` is the value you want to assign to it. Once set, `myVar` can be accessed in later tasks using the `$(myVar)` syntax.

For example, in your pipeline YAML:

```yaml
- script: |
    MY_VALUE="Hello, Pipeline!"
    echo "##vso[task.setvariable variable=myVar]$MY_VALUE"
  name: task1
  displayName: 'Set pipeline variable from script'

- script: |
    echo "The value of myVar is: $(task1.myVar)"
  displayName: 'Use pipeline variable'
```

In this scenario, the first script sets the value of `myVar`, which is then used in the second script.

# Part 4. Exchanging Variables Between Tasks

## a. Variable Scope and Lifespan

In Azure Pipelines, variables defined within a task are automatically available to subsequent tasks in the same job. These variables can be used not just within the scripts of those tasks, but also in the YAML configuration of later tasks. This makes it easy to dynamically adjust the behavior of your pipeline based on the outputs of previous tasks.

## b. Practical Examples

Let’s look at a few examples on how to consume the variable a task is outputting.

### How to set a variable

```yaml
- task: Bash@3
  name: GenerateVar
  inputs:
    targetType: 'inline'
    script: |
      myVar="HelloWorld"
      echo "##vso[task.setvariable variable=myVar]$myVar"
  displayName: 'Generate a variable'
 ```

In this task, we set a variable named `myVar` with the value `"HelloWorld"`. The echo command outputs a special string that Azure Pipelines recognizes, storing `myVar` as a pipeline variable. The task is named `GenerateVar` for reference in subsequent steps.

### How to use it in a script of a Task

```yaml
- task: Bash@3
  inputs:
    targetType: 'inline'
    script: |
      echo "The variable is: $(myVar)"
  displayName: 'Use the variable from previous task'
```
This task accesses the `myVar` variable set by the previous task and prints its value. The `$(myVar)` syntax is used to reference the variable within the script.

### How to use in YAML

```yaml
- task: Bash@3
  inputs:
    myVar:  $(GenerateVar.myVar)
    script: |
      ...
  displayName: 'Use the variable from previous task'
```

Here, the variable `myVar` from the `GenerateVar` task is directly referenced using `$(GenerateVar.myVar)`. This allows you to use the value of `myVa`r in subsequent tasks, either within the script or as an input parameter.

### How to use in conditions in YAML

```yaml
- task: Bash@3
  name: ConditionalStep
  condition: eq(variables['myVar'], 'HelloWorld')
  inputs:
    targetType: 'inline'
    script: |
     ...
  displayName: 'Conditional execution based on variable'
```
In this example, the `ConditionalStep` task will only execute if the value of `myVar` is equal to `HelloWorld`. The `condition` field is used to create a conditional step based on the value of a pipeline variable.

# Part 5. Exchanging Variables Between Jobs

## a. Job Dependencies and Variable Sharing

In Azure Pipelines, jobs are often designed to be independent units of work. However, there are scenarios where you need to share variables between these jobs, particularly when the output of one job is necessary for the execution of another. This is where job dependencies and variable sharing come into play.

### Using `dependsOn` and `condition`

The `dependsOn` attribute allows you to specify that a job should only run after one or more other jobs have successfully completed. This attribute is essential when you want to ensure that a downstream job only executes after its prerequisite jobs have finished processing.

For variable sharing between jobs, the `condition` attribute is used to control the execution of a job based on the outcome of another job. This is useful for scenarios where you want to run a job only if certain conditions are met in a previous job.

For example, consider the following YAML snippet:

```yaml
jobs:
- job: JobA
  steps:
  - script: echo "##vso[task.setvariable variable=JobAVar]JobAOutput"
    name: SetJobAVar

- job: JobB
  dependsOn: JobA
  condition: succeeded()  # Only runs if JobA succeeds
  steps:
  - script: echo "JobA output was $(JobAVar)"
```

In this example, `JobB` depends on `JobA` and only runs if `JobA` is successful. The variable `JobAVar` is set in `JobA` and then accessed in `JobB` using the `$(JobAVar)` syntax.


## b. Job Output as Input

There are many scenarios where the output from one job is essential for the execution of another. Azure Pipelines allows you to capture and pass this output between jobs seamlessly.

### Using Job Outputs

To use the output of one job as the input to another, you can leverage the `jobOutputs` feature. This allows you to define outputs in a job that can be accessed by downstream jobs.

Here’s a practical example:

```yaml
jobs:
- job: JobA
  steps:
  - script: echo "JobA output"
    name: SetJobAOutput
  - script: echo "##vso[task.setvariable variable=JobAResult;isOutput=true]JobAOutput"

- job: JobB
  dependsOn: JobA
  variables:
    JobBInput: $[ dependencies.JobA.outputs['SetJobAOutput.JobAResult'] ]
  steps:
  - script: echo "JobA result: $(JobBInput)"
```

In this example, `JobA` sets a variable `JobAResult` and marks it as output using `isOutput=true`. In `JobB`, this output is accessed through the `dependencies` context, allowing `JobB` to use the value of `JobAResult`.

### Practical Example with a Multi-Job Pipeline Setup

Consider a more comprehensive pipeline where the output from one job is critical for determining the behavior of another job. For instance, imagine a pipeline that first compiles code, then runs tests, and finally deploys the application only if the tests pass.

```yaml
jobs:
- job: Compile
  steps:
  - script: echo "Compiling code..."
  - script: echo "##vso[task.setvariable variable=BuildOutput;isOutput=true]CompiledCode"

- job: Test
  dependsOn: Compile
  variables:
    TestInput: $[ dependencies.Compile.outputs['Compile.BuildOutput'] ]
  steps:
  - script: echo "Testing $(TestInput)"
  - script: exit 1  # Simulating a test failure

- job: Deploy
  dependsOn:
    - Compile
    - Test
  condition: succeeded()
  variables:
    DeployInput: $[ dependencies.Compile.outputs['Compile.BuildOutput'] ]
  steps:
  - script: echo "Deploying $(DeployInput)"
```

In this setup:

- The `Compile` job compiles the code and outputs `CompiledCode`.
- The `Test` job runs tests using the compiled code. If the tests fail, `Deploy` won’t run due to the `condition: succeeded()` setting.
- If the tests pass, the `Deploy` job uses the same `CompiledCode` for deployment.

This example demonstrates a typical scenario where jobs are linked, and variables are passed from one to the next, ensuring that the entire pipeline behaves as expected based on the outputs of individual jobs.

## Persisting Variables Across Jobs Using Pipeline Artifacts

Another method to share data between jobs is through pipeline artifacts. Artifacts are files or directories created during the execution of a job that can be passed to subsequent jobs. This approach is especially useful for sharing larger sets of data or complex outputs that can’t be easily managed as simple variables.

Here’s how you can persist variables across jobs using artifacts:

```yaml
jobs:
- job: JobA
  steps:
  - script: echo "JobA output" > output.txt
  - publish: output.txt
    artifact: JobAOutput

- job: JobB
  dependsOn: JobA
  steps:
  - download: current
    artifact: JobAOutput
  - script: cat $(Pipeline.Workspace)/JobAOutput/output.txt
```

In this example, `JobA` generates a file `output.txt`, which is then published as an artifact. `JobB` downloads this artifact and can then use the contents of `output.txt` in its execution.

# Part 6. Exchanging Variables Between Stages

## a. Stage-Level Variable Sharing

In Azure Pipelines, stages represent major steps in your CI/CD pipeline, such as build, test, and deploy phases. Sharing variables between these stages is essential when you need to pass data or state information across different stages of your pipeline. However, since each stage runs in isolation, direct variable sharing between stages is not as straightforward as within tasks or jobs.

To share variables between stages, Azure Pipelines provides mechanisms like pipeline context and artifacts.

- **Pipeline Context**: Pipeline context refers to the overall environment and data that can be accessed throughout the pipeline execution. While variables defined in one stage aren't directly accessible in another, you can pass information via pipeline context. For instance, you can set variables in one stage and then access them in subsequent stages using the `stageDependencies` and `dependencies` syntax.

- **Pipeline Artifacts**: Artifacts are files or data generated during a pipeline run that you can store and share between stages. By using artifacts, you can persist output from one stage and retrieve it in another. For example, a build stage might produce an artifact that is later consumed by a deployment stage. Artifacts ensure that the necessary data is available when needed, even across isolated stages.

## b. Example Pipeline with Multi-Stage Variable Sharing

Let’s consider a multi-stage pipeline where a variable is defined in the build stage and used in the deployment stage. 

**Example YAML Pipeline:**

```yaml
stages:
- stage: Build
  jobs:
  - job: BuildJob
    steps:
    - script: |
        echo "##vso[task.setvariable variable=BUILD_NUMBER;isOutput=true]$(Build.BuildId)"
      name: SetBuildNumber
    - publish: $(Pipeline.Workspace)/output
      artifact: buildOutput

- stage: Deploy
  dependsOn: Build
  jobs:
  - job: DeployJob
    variables:
      BUILD_NUMBER: $[ dependencies.Build.jobs.BuildJob.outputs['SetBuildNumber.BUILD_NUMBER'] ]
    steps:
    - download: current
      artifact: buildOutput
    - script: |
        echo "Deploying build number $(BUILD_NUMBER)"
```

**Explanation**:

- **Build Stage**: In the `Build` stage, we run a script that sets a variable `BUILD_NUMBER` using the special `##vso[task.setvariable]` syntax. The `isOutput=true` flag allows this variable to be referenced in subsequent stages. We also publish an artifact named `buildOutput` that can be used later.

- **Deploy Stage**: The `Deploy` stage depends on the `Build` stage and retrieves the `BUILD_NUMBER` variable from the `BuildJob`. This variable is then used within the `DeployJob` to echo the build number. The `download` step retrieves the artifact produced in the `Build` stage, ensuring the deployment has the necessary build files.

### Best Practices for Managing Variables in Complex Pipelines:

- **Consistent Naming Conventions**: Use consistent and descriptive names for variables to avoid confusion when passing data between stages.
- **Use Artifacts for Larger Data**: If you're passing large or complex data between stages, prefer artifacts over variables to maintain clarity and efficiency.
- **Document Variable Flow**: Clearly document which variables are set and used in each stage, especially in large pipelines with multiple stages. This helps in troubleshooting and maintaining the pipeline.
- **Use Secure Files and Secrets**: For sensitive data, consider using secure files or Azure Key Vault to manage secrets rather than passing them as plain text variables.


# Conclusion

Working with variables in Bash scripting and Azure Pipelines can seem complicated at first, especially with the different ways symbols like `$`, `(`, `[`, and `{` are used. However, like any skill, it becomes easier with practice. The more you work with these concepts, the more intuitive they will become. 
