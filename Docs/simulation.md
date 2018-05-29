# Simulation Mode Snippet

```csharp
    string absPathToMockFolder = .... 
    // in case of simulation you can put fake credentials 
    // it's going to ignore them
    var config = new Marketplace.ClientConfig("test", "test-key");
    var apiClient = new Marketplace.ApiClient(config);
    apiClient.SimulationEnabled = true;



    // it's also possible to specify your own folder for mock files
    // this folder should have _mapping.json file 
    // which works as a router for all requests
    // it maps request to an appropriate file 
    // by choosing the longest key \
    var fetcher = (LocalFetcher) apiClient.GetHttpHandler().Fetcher;
    fetcher.SetCustomMockFolder(absPathToMockFolder);



    feedApi = new Marketplace.V3.Api.FeedEndpoint(apiClient);
    // all responses now served from local files
```