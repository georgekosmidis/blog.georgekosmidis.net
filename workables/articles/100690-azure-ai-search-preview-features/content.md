When I first started working with Azure Search back when it was still called, well, *Azure Search*, the mental model was simple: throw some documents at it, get a keyword-ranked list back. Done. Fast-forward a decade and the service has gone through more name changes than a Marvel character - Azure Search &gt; Azure Cognitive Search &gt; Azure AI Search - and every rename has come with a serious leap in capability.

The latest preview API version, **2025-11-01-preview**, is no different. There is a lot going on. From agentic retrieval to document-level access control, the preview features list reads less like a changelog and more like a roadmap for the next generation of AI-powered search experiences. Let's break it all down.


## A quick look at what "preview" means here

Azure AI Search ships two kinds of previews: **data plane** (things that affect how you index and query content) and **control plane** (things that affect how you manage the service itself). Preview API versions are cumulative - each one rolls up everything from the previous preview - so using the latest one is always the safest bet.

> The current latest previews are: `2025-11-01-preview` for data plane operations and `2025-05-01-preview` for management operations. Always check the [official preview features list](https://learn.microsoft.com/en-us/azure/search/search-api-preview) as these change.


## The headline act: Agentic Retrieval

If there is one feature in this preview round that changes the game, it is **agentic retrieval**. Classical search is transactional: one query in, one ranked list out. Agentic retrieval, by contrast, is conversational and LLM-powered - it breaks a complex user question into multiple sub-queries, runs them in parallel, reranks the results, and synthesises a grounded answer in natural language.

Under the hood, you set up a **knowledge base** that can draw on multiple **knowledge sources**: your Azure AI Search index, web data, SharePoint, or OneLake. The response payload keeps full transparency on the query plan and the reference chunks used - which is great for debugging and for RAG pipelines that need attribution.

### Step 1: Create a knowledge base

```http
POST https://<your-service>.search.windows.net/knowledgebases?api-version=2025-11-01-preview
Content-Type: application/json

{
  "name": "my-kb",
  "description": "Knowledge base over product docs",
  "defaultRerankerThreshold": 0.5
}
```

### Step 2: Add a knowledge source

```http
POST https://<your-service>.search.windows.net/knowledgebases/my-kb/knowledgesources?api-version=2025-11-01-preview
Content-Type: application/json

{
  "name": "product-docs-index",
  "type": "AzureAISearch",
  "indexName": "products"
}
```

### Step 3: Retrieve

```http
POST https://<your-service>.search.windows.net/knowledgebases/my-kb/retrieve?api-version=2025-11-01-preview
Content-Type: application/json

{
  "query": "Which products support offline mode and have a battery life over 20 hours?",
  "topK": 5,
  "synthesize": true
}
```

> If you prefer to click before you code, the Azure portal now has a guided experience for agentic retrieval that walks you through knowledge base creation end-to-end. It is a good way to get a feel for the query plan output.


## The full preview feature rundown

Beyond agentic retrieval, there are a dozen other features worth knowing about. Here is the full list, grouped by what they do for you.

### Smarter relevance and ranking

- **Scoring function aggregation** - You can now combine and weight multiple scoring functions inside a single scoring profile, rather than letting only one "win". Think of it as a weighted average of signals: BM25 score, freshness boost, geo-proximity boost, all blended together with explicit weights.

- **Query rewrite in the semantic reranker** - The L2 semantic reranker can now optionally rewrite (and expand) the original query before re-ranking, producing more relevant results from ambiguous or short queries. Enable it by adding a `queryRewrite` object to your semantic query.

- **flightingOptIn in semantic configurations** - Want to be the first on your block to try pre-release semantic ranking models? Set `flightingOptIn: true` in your semantic configuration and Azure will enrol your service into preview model evaluations if one is available in your region.

### Richer faceting and analytics

- **Facet aggregations** - The classic facet just gave you a count per bucket. Now you can ask for `sum`, `min`, `max`, `average`, and `cardinality` aggregations too. Perfect for things like "show me the average price per category" right inside the facet response.

- **Facet hierarchies and facet filters** - Nested facets are here. You can now drill down through a hierarchy (e.g., `Category > Subcategory`) and apply inclusion/exclusion filters directly on a facet expression, all in a single search request.

### Better vector search

- **Strict post-filtering (`strictPostFilter`)** - Previously, applying a filter to a vector query could shrink the candidate set *before* the ANN algorithm ran, potentially missing relevant results. The new `strictPostFilter` mode flips this: ANN runs first, finds the global top-K, then the filter is applied. The result is always a subset of the unfiltered vector results - no surprises.

- **Multivector fields** - You can now store *multiple* child vectors inside a single document field using complex collections. Useful for documents with several independently embeddable sections (abstract, body, conclusion) that you want to keep associated with one parent document.

- **Hybrid search filter override (`filterOverride`)** - In a hybrid query (keyword + vector), filters used to apply globally to all sub-queries. The new `filterOverride` parameter lets you scope a filter to only the vector sub-query (or only the keyword sub-query), giving you much finer control.

### Security and governance

- **Document-level access control** - Finally! Permissions from Azure Data Lake Storage Gen2 blobs can now flow through to the search index. Queries automatically filter results based on the calling user's identity, so you do not need to handle row-level security yourself in a post-query step.

- **Purview sensitivity labels** - Microsoft Purview classifications and sensitivity labels from source metadata can now be applied during indexing. This is mostly for regulated industries where the governance story of your search index needs to match the governance story of the source data.

### AI skills and enrichment

- **GenAI Prompt skill** - A new skill that wires an LLM directly into the indexing pipeline via a prompt you define. The canonical use case is *image verbalization*: the skill sends an image to a multimodal model, gets back a text description, and writes it to a searchable field. No custom Web API skill needed.

- **Azure Vision multimodal embedding skill** - Calls the Azure Vision multimodal API during indexing to produce embeddings for both text and image content. Useful for image-rich catalogues where you want unified text-and-image vector search.

- **AML skill + Foundry model catalog** - The Azure Machine Learning skill now works with models deployed from the **Microsoft Foundry model catalog**, not just custom AML endpoints. This opens up a much larger library of embedding and fine-tuned models you can plug directly into your skillset.

- **Text Split skill token chunking** - The existing text split skill gets a new `unit: token` mode with a configurable tokenizer and a `doNotSplitOn` list. Chunking by token count (rather than character count) is a much better fit for embedding models, which care about token boundaries, not raw characters.

- **Keyless billing for AI skills** - You can now use a managed identity + RBAC to connect the skills pipeline to Foundry Tools, removing the requirement that Azure AI Search and your Foundry resource live in the same region. No more API key passing.

- **Markdown parsing mode** - Indexers can now parse Markdown files stored in Azure Blob Storage in one-to-one or one-to-many mode (one search document per file, or one per heading section). Structure-aware chunking for free.

### Observability

- **Improved indexer runtime tracking** - The Get Service Statistics and Get Indexer Status endpoints now expose cumulative processing information: total documents processed, total errors, throughput over time. Useful for capacity planning and catching silent failures in long-running indexers.

## Control plane: pricing tier changes (preview)

On the management side, the `2025-05-01-preview` adds the ability to **change a service's pricing tier** and perform a **self-service upgrade** of an existing service - without recreating it. This is big for teams that started on Basic and are now hitting storage or replica limits. The feature is available via the management REST API:

```http
PATCH https://management.azure.com/subscriptions/{subId}/resourceGroups/{rg}/providers/
      Microsoft.Search/searchServices/{serviceName}?api-version=2025-05-01-preview
Content-Type: application/json

{
  "sku": {
    "name": "standard2"
  }
}
```

> Service tier changes can take several minutes and may require a brief interruption in query availability. Test in non-production first.


## Where should you start?

If you are building a RAG or agent application today, **agentic retrieval** is the obvious first stop. It replaces a lot of glue code that teams are currently writing manually (the query-decompose → search → rerank → synthesize loop).

If you already have a working RAG pipeline and want to improve result quality, the **query rewrite** feature and **strict post-filtering** for vectors are both low-effort wins. And if you index blob content that has sensitivity classifications in Purview, the new label propagation feature is worth evaluating now before it goes GA and you have no excuse not to use it.

All of these features are accessible via the `2025-11-01-preview` API version. The [official preview feature list](https://learn.microsoft.com/en-us/azure/search/search-api-preview) on Microsoft Learn is the single best place to track what is landing next.


## That's it!

Azure AI Search has quietly become one of the most feature-rich services in the Azure AI portfolio. The preview pipeline is moving fast - agentic retrieval alone represents a fundamental rethink of how search fits into AI application architecture. Keep an eye on it, and don't wait for GA to start experimenting.