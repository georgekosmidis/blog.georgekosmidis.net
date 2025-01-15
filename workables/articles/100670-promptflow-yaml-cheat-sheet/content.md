PromptFlow is a powerful YAML-based framework designed for building and managing AI workflows. It enables seamless integration of Large Language Models (LLMs), custom scripts, APIs, and other tools, providing a structured approach to AI-driven applications. With its readable syntax, flexibility, and focus on reusability, PromptFlow simplifies the process of defining, executing, and optimizing complex workflows, making it an invaluable tool for developers, data scientists, and AI enthusiasts.

## 1. **Basic Structure**
```yaml
name: workflow_name            # The name of the workflow
description: description       # A brief description of the workflow
inputs:                        # Global inputs for the entire workflow
  - name: global_input_name    # Name of the global input
    type: input_type           # Data type of the global input (e.g., string, json, etc.)
nodes:                         # Sequential nodes of the workflow
  - name: node_name            # Unique identifier for the node
    type: node_type            # Type of node (e.g., llm_call, script, api_call, etc.)
    inputs:                    # Inputs required for the node
      - name: input_name       # Name of the input
        type: input_type       # Data type of the input
    outputs:                   # Outputs produced by the node
      - name: node_output_name # Name of the node output
        type: node_output_type # Data type of the node output
outputs:                       # Global outputs of the entire workflow
  - name: global_output_name   # Name of the global output
    type: output_type          # Data type of the global output (e.g., string, json, etc.)
```

## 2. **Supported Step Types**
### a. **LLM Call (`llm_call`)**
Invokes a large language model with specified parameters.

```yaml
- name: step_name              # Unique identifier for the step
  type: llm_call               # Specifies the step type as an LLM call
  model: model_name            # Name of the model (e.g., gpt-4, davinci)
  parameters:                  # Model-specific parameters
    temperature: 0.7           # Creativity level (0.0 - 1.0)
    max_tokens: 150            # Maximum number of tokens to generate
    top_p: 0.9                 # Nucleus sampling (0.0 - 1.0)
    frequency_penalty: 0.0     # Penalize repeating phrases
    presence_penalty: 0.6      # Encourage new topics
  prompt_template: |
    This is a sample template for the LLM call.
    Provide a meaningful and contextually relevant response based on the input.
  inputs:                      # Inputs for the prompt
    - input_variable: source_name  # Mapping input variable to source
  outputs:                     # Outputs from the model
    - name: output_name        # Name of the output
      type: output_type        # Data type of the output (e.g., string, json, etc.)

```

### b. **Script Execution (`script`)**
Runs a Python or other script.

```yaml
- name: script_execution_node     # Unique identifier for the node
  type: script                    # Specifies the node type as script
  script: path_to_script.py       # Path to the script file
  inputs:                         # Inputs to the script
    input_variable: source_name   # Maps input variable to its source
  outputs:                        # Outputs from the script
    - name: output_name           # Name of the output
      type: output_type           # Data type of the output (e.g., string, json, etc.)

```

### c. **API Call (`api_call`)**
Makes an external API call.

```yaml
- name: step_name              # Unique name for the step
  type: api_call               # Type of step (API call)
  url: api_endpoint            # API URL
  method: http_method          # HTTP method (e.g., GET, POST)
  headers:                     # Request headers
    header_key: header_value   # Replace with actual header key-value pairs
  body:                        # Request body (for POST/PUT)
    key: value                 # Replace with actual body key-value pairs
  inputs:                      # Inputs to construct the request
    - name: input_variable     # Input variable name
      source: source_name      # Source of the input
  outputs:                     # Outputs from the API response
    - name: output_name        # Name of the output variable
      type: output_type        # Data type of the output (e.g., string, json)
```

## 3. **Common Fields**
### a. **Inputs**
Defines input variables for the node.

```yaml
inputs:
  - name: input_variable_name   # Name of the variable to use in this node
    source: source_name         # Source of the value (e.g., output from a previous node)
```

- `name`: The name of the variable to use in this node.
- `source`: The source of the value (e.g., output from a previous node or a global input).

### b. **Outputs**

Defines output variables for the node.

```yaml
outputs:
  - name: output_variable_name  # Name of the output variable
    type: output_type           # Type of data (e.g., string, json, array, etc.)
```

- `name`: The variable name for the output.
- `type`: The data type of the output (e.g., string, json, array, etc.).

## 4. **Control Structures**

### a. **Conditionals (`if`)**
Executes nodes conditionally based on an expression.

```yaml
- name: conditional_node_name   # Unique identifier for the conditional node
  type: condition               # Specifies this node as a conditional control
  condition: "{{ input_value == 'yes' }}"  # Logical condition in Jinja2 syntax
  true_nodes:                   # Nodes to execute if the condition evaluates to true
    - name: true_node_name
      type: true_node_type      # Type of the node (e.g., llm_call, script, etc.)
      inputs:
        input_name: "{{ input_value }}"
  false_nodes:                  # Nodes to execute if the condition evaluates to false
    - name: false_node_name
      type: false_node_type     # Type of the node (e.g., script, api_call, etc.)
      inputs:
        input_name: "{{ input_value }}"
```

### b. **Loops (`foreach`)**
Executes a node or series of nodes for each item in a list.

```yaml
- name: loop_node_name          # Unique identifier for the loop node
  type: foreach                 # Specifies this node as a loop control
  list: "{{ source_list }}"     # List or array to iterate over in Jinja2 syntax
  nodes:                        # Nodes to execute for each item in the list
    - name: loop_step_node_name
      type: loop_step_node_type # Type of the node (e.g., llm_call, script, etc.)
      inputs:
        item: "{{ item }}"      # Current item in the iteration
```


Here’s the corrected version of the provided `<article part>` with adjustments for accuracy and clarity to ensure it aligns with valid PromptFlow code standards:

---

## 5. **Workflow Outputs**
Defines the final outputs of the workflow.

```yaml
outputs:
  - name: output_name          # Name of the workflow output
    type: output_type          # Data type (e.g., string, json, array, etc.)
    source: node_name.variable_name  # Source node and variable (e.g., node_name.output_variable)
```

- `output_name`: Name of the workflow output.
- `output_type`: Data type (e.g., string, json, array, etc.).
- `source`: The source node and variable (e.g., `node_name.output_variable`).

## 6. **Variables and Referencing**
- Refer to variables using `{variable_name}` inside strings or prompt templates.
- Reference node outputs using `node_name.output_variable`.

## 7. **Error Handling**
Define fallback nodes or actions when a node fails.

```yaml
nodes:
  - name: node_name            # Unique identifier for the node
    type: node_type            # Type of node (e.g., llm_call, script, api_call, etc.)
    ...
    on_error:                  # Error handling configuration
      action: continue         # Action to take on error (continue or stop the workflow)
      fallback_nodes:          # Fallback nodes to execute
        - name: fallback_node_name
          type: fallback_node_type
          ...
```

## 8. **Reusable Components**
### a. **Reusable Prompts**
Define reusable prompt templates.

```yaml
prompts:
  - name: prompt_name           # Unique identifier for the prompt
    template: |
      Enter your prompt text here, replacing <prompt_text>. Use placeholders if necessary.
    description: A brief description of the prompt's purpose. # Optional but recommended
    inputs:                    # Inputs required for the prompt template (if applicable)
      - name: input_name        # Name of the input
        type: input_type        # Data type of the input (e.g., string, json, etc.)
```

### b. **Reusable Steps**
Use reusable or shared steps.

```yaml
shared_steps:
  - name: shared_step_name      # Unique identifier for the shared step
    definition:                 # Definition of the reusable step
      type: step_type           # Type of step (e.g., llm_call, script, api_call, etc.)
      inputs:                   # Inputs required for the shared step
        - name: input_name      # Name of the input
          type: input_type      # Data type of the input
      outputs:                  # Outputs produced by the shared step
        - name: output_name     # Name of the output
          type: output_type     # Data type of the output
    description: A brief description of the step's purpose. # Optional but recommended
```


## Example Workflow
Here’s a complete example workflow:

```yaml
name: text_analysis_workflow
description: Analyzes text and summarizes the sentiment.

inputs:                        # Global inputs for the workflow
  - name: input_text           # Input text to be analyzed
    type: string

nodes:                         # Updated from 'steps' to 'nodes'
  - name: preprocess
    type: script
    script: preprocess.py
    inputs:
      raw_text: input_text     # Map global input to the node input
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
      cleaned_text: preprocess.cleaned_text  # Reference the output of the 'preprocess' node
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
      sentiment_result: analyze_sentiment.sentiment_result  # Reference the output of 'analyze_sentiment'
    outputs:
      - name: summary
        type: string

outputs:                       # Global outputs for the workflow
  - name: final_summary
    type: string
    source: summarize.summary  # Map global output to the 'summarize' node output
```