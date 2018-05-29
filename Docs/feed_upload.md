# Order List Snippet 

Here is an example of how to get a list of all orders using SDK.
You can check [SDK Initialization]() to see an example on how to configure SDK

```csharp
    string absPath = ...
    ApiClient client = ... // check SDK Init snippet
    var feedEndpoint = new Walmart.Sdk.Marketplace.V3.Api.feedEndpoint(client);

    using (var feedStream = File.OpenRead(absPath))
    {
        try
        {
            var feedAck = await feedEndpoint.UploadFeed(feedStream);
            // ... processing result
        }
        catch (V3.Api.Exception.ApiException apiEx)
        {
            // process error from API
            // you can see list of possible errors here
            // https://developer.walmart.com/#/apicenter/marketPlace/latest#errors
        }
    }
    
```