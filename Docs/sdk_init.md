# SDK Init Snippet

This snippet gives you an example of SDK initialization, you need to do this step before you can use any of the available endpoints.

```csharp
    Dictionary<string, string> settings = ...;

    config = new Marketplace.ClientConfig(
        settings["ConsumerId"],
        settings["PrivateKey"]
    );
    config.BaseUrl = settings["BaseUrl"];
    config.ChannelType = settings["ChannelType"];
    config.ServiceName = settings["ServiceName"];
    client = new ApiClient(config);

    var orderEndpoint = new Walmart.Sdk.Marketplace.V2.Api.OrderEndpoint(client);
```