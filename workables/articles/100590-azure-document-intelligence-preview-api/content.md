### Title: Enhancing Azure Document Intelligence API with Custom API Version Overrides

### Introduction
The Azure Document Intelligence API offers powerful capabilities for form recognition and document analysis. However, the client library for JavaScript that offers ease of usage might not always align with your needs, especially when it comes to using preview versions of the API. In this blog post, we'll explore how to override the default API version used by the Azure Document Intelligence SDK to leverage the latest preview features.

### Why Override the Default API Version?
- **Access to New Features**: Preview versions often include cutting-edge features not yet available in the stable release.
- **Customization**: Tailor the API behavior to suit specific project requirements.
- **Future-proofing**: Stay ahead of the curve by testing and integrating new capabilities early.

### Steps to Implement Custom API Version Override

#### 1. Creating the Custom Policy Class
The first step is to create a custom policy that overrides the default API version in the request URL.

```typescript
import { PipelinePolicy, PipelineRequest, PipelineResponse, SendRequest } from '@azure/core-rest-pipeline';

export class OverrideCustomApiVersionPolicy implements PipelinePolicy {
  name: string = 'OverrideCustomApiVersionPolicy';
  constructor(private apiVersion: string) {}

  sendRequest(request: PipelineRequest, next: SendRequest): Promise<PipelineResponse> {
    const url = new URL(request.url);
    url.searchParams.set('api-version', this.apiVersion);
    request.url = url.toString().replace('formrecognizer', 'documentintelligence');
    var response = next(request);
    console.info(`Azure Document Intelligence Request: ${request.url}`);
    return response;
  }
}
```

#### 2. Overriding the Default DocumentAnalysisClient
Next, we'll override the default `DocumentAnalysisClient` to incorporate our custom policy.

```typescript
import { DocumentAnalysisClient } from '@azure/ai-form-recognizer';
import { DocumentAnalysisClientOptionsPreview } from './DocumentAnalysisClientOptionsPreview';
import { OverrideCustomApiVersionPolicy } from './OverrideCustomApiVersionPolicy';
import { TokenCredential } from '@azure/identity';

export class DocumentAnalysisClientPreview extends DocumentAnalysisClient {
  constructor(endpoint: string, credential: TokenCredential, options: DocumentAnalysisClientOptionsPreview) {
    super(endpoint, credential, options);
    if (!options.additionalPolicies) {
      options.additionalPolicies = [];
    }
    const policy = new OverrideCustomApiVersionPolicy(options.apiVersion);

    // Add policy to the end of the pipeline to override the URL
    // @ts-ignore
    this._restClient.pipeline.addPolicy(policy);
  }
}
```

#### 3. Defining Custom Options
We also need to define a custom options interface to include the API version.

```typescript
import { DocumentAnalysisClientOptions } from '@azure/ai-form-recognizer';

export interface DocumentAnalysisClientOptionsPreview extends DocumentAnalysisClientOptions {
  apiVersion: string;
}
```

#### 4. Using the Custom Client
Finally, we instantiate and use the custom client in our application.

```javascript
const client = new DocumentAnalysisClientPreview(
  process.env.AZURE_DOCUMENT_INTELLIGENCE_ENDPOINT, 
  tokenResolver.getToken(), 
  { apiVersion: '2024-05-01-preview' }
);
```

### Conclusion
Although not perfect since it does not include potential model changes, implementing a custom API version override allows you to unlock preview versions of the Azure Document Intelligence API, access the latest features, and ensure your application remains at the forefront of technology - at least during development. 
Just keep in mind that this is NOT a production solution, as preview versions can change or be withdrawn without notice!