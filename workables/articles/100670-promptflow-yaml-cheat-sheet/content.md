PromptFlow is a powerful YAML-based framework designed for building and managing AI workflows. It enables seamless integration of Large Language Models (LLMs), custom scripts, APIs, and other tools, providing a structured approach to AI-driven applications. With its readable syntax, flexibility, and focus on reusability, PromptFlow simplifies the process of defining, executing, and optimizing complex workflows, making it an invaluable tool for developers, data scientists, and AI enthusiasts.

## 1. **Basic Structure**
```yaml
name: <workflow_name>          # The name of the workflow
description: <description>     # A brief description of the workflow
steps:                         # Sequential steps of the workflow
  - name: <step_name>          # Unique identifier for the step
    type: <step_type>          # Type of step (e.g., llm_call, script, api_call, etc.)
    ...
outputs:                       # Final outputs of the workflow
  - name: <output_name>        # Name of the output
    type: <output_type>        # Data type (e.g., string, json, etc.)
```

## 2. **Supported Step Types**
### a. **LLM Call (`llm_call`)**
Invokes a large language model with specified parameters.

```yaml
- name: <step_name>
  type: llm_call
  model: <model_name>          # Name of the model (e.g., gpt-4, davinci)
  parameters:                  # Model-specific parameters
    temperature: <float>       # Creativity level (0.0 - 1.0)
    max_tokens: <int>          # Maximum number of tokens to generate
    top_p: <float>             # Nucleus sampling (0.0 - 1.0)
    frequency_penalty: <float> # Penalize repeating phrases
    presence_penalty: <float>  # Encourage new topics
  prompt_template: |
    <template_text>            # Multiline template for the prompt
  inputs:                      # Inputs for the prompt
    - <input_variable>: <source_name>
  outputs:                     # Outputs from the model
    - name: <output_name>
      type: <output_type>
```

### b. **Script Execution (`script`)**
Runs a Python or other script.

```yaml
- name: <step_name>
  type: script
  script: <path_to_script>     # Path to the script file
  inputs:                      # Inputs to the script
    - <input_variable>: <source_name>
  outputs:                     # Outputs from the script
    - name: <output_name>
      type: <output_type>
```

### c. **API Call (`api_call`)**
Makes an external API call.

```yaml
- name: <step_name>
  type: api_call
  url: <api_endpoint>          # API URL
  method: <http_method>        # HTTP method (e.g., GET, POST)
  headers:                     # Request headers
    <header_key>: <header_value>
  body:                        # Request body (for POST/PUT)
    <key>: <value>
  inputs:                      # Inputs to construct the request
    - <input_variable>: <source_name>
  outputs:                     # Outputs from the API response
    - name: <output_name>
      type: <output_type>
```

## 3. **Common Fields**
### a. **Inputs**
Defines input variables for the step.

```yaml
inputs:
  - <input_variable>: <source_name>
```
- `<input_variable>`: Name of the variable to use in this step.
- `<source_name>`: Source of the value (e.g., previous step output).

### b. **Outputs**
Defines output variables for the step.

```yaml
outputs:
  - name: <output_name>
    type: <output_type>
```
- `<output_name>`: Variable name for the output.
- `<output_type>`: Type of data (e.g., string, json, array, etc.).

## 4. **Control Structures**
### a. **Conditionals (`if`)**
Executes steps conditionally based on an expression.

```yaml
- name: <step_name>
  type: condition
  condition: <expression>      # Logical condition (e.g., "input_value == 'yes'")
  true_steps:                  # Steps to execute if condition is true
    - <step_definition>
  false_steps:                 # Steps to execute if condition is false
    - <step_definition>
```

### b. **Loops (`foreach`)**
Executes a step or series of steps for each item in a list.

```yaml
- name: <step_name>
  type: foreach
  list: <source_list>          # List to iterate over
  steps:                       # Steps to execute for each item
    - <step_definition>
```

## 5. **Workflow Outputs**
Defines the final outputs of the workflow.

```yaml
outputs:
  - name: <output_name>
    type: <output_type>
    source: <step_output>
```
- `<output_name>`: Name of the workflow output.
- `<output_type>`: Data type (e.g., string, json, array, etc.).
- `<step_output>`: The source step and variable (e.g., `step_name.variable_name`).

## 6. **Variables and Referencing**
- Refer to variables using `{variable_name}` inside strings or prompt templates.
- Reference step outputs using `<step_name>.<output_variable>`.

## 7. **Error Handling**
Define fallback steps or actions when a step fails.

```yaml
- name: <step_name>
  type: <step_type>
  ...
  on_error:                    # Error handling configuration
    action: <continue/stop>    # Continue or stop the workflow
    steps:                     # Fallback steps to execute
      - <step_definition>
```



## 8. **Reusable Components**
### a. **Reusable Prompts**
Define reusable prompt templates.

```yaml
prompts:
  - name: <prompt_name>
    template: |
      <prompt_text>
```

### b. **Reusable Steps**
Use reusable or shared steps.

```yaml
shared_steps:
  - name: <shared_step_name>
    definition:
      <step_definition>
```

## Example Workflow
Here’s a complete example workflow:

```yaml
name: text_analysis_workflow
description: Analyzes text and summarizes the sentiment.

steps:
  - name: preprocess
    type: script
    script: preprocess.py
    inputs:
      - raw_text: input_text
    outputs:
      - name: cleaned_text
        type: string

  - name: analyze_sentiment
    type: llm_call
    model: gpt-4
    parameters:
      temperature: 0.5
      max_tokens: 150
    prompt_template: |
      Analyze the sentiment of the following text:
      ---
      {cleaned_text}
    inputs:
      - cleaned_text: preprocess.cleaned_text
    outputs:
      - name: sentiment_result
        type: string

  - name: summarize
    type: llm_call
    model: gpt-4
    parameters:
      temperature: 0.7
      max_tokens: 50
    prompt_template: |
      Summarize the following sentiment analysis:
      ---
      Sentiment: {sentiment_result}
    inputs:
      - sentiment_result: analyze_sentiment.sentiment_result
    outputs:
      - name: summary
        type: string

outputs:
  - name: final_summary
    type: string
    source: summarize.summary
```


Well that's it, no epilogue here :)