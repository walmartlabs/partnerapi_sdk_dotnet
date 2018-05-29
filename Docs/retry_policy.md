# Retry Policy Snippet


```csharp

    ApiClient apiClient = ... // check SDK init snippet for more details
    apiClient.RetryPolicy = new CustomRetryPolicy();

```

Retry Policy class example
```csharp

public class CustomRetryPolicy: BasePolicy
{
    public override async Task<IResponse> GetResponse(Http.Fetcher.IFetcher fetcher, IRequest request)
    {
        for (var i=0; i<10; i++)
        {
            // give it a try
            if (await ExecuteOnce(fetcher, request))
                return response;

            // give it a break before another retry
            await Task.Delay(1000);
        }
        throw Http.Exception.NoRetriesLeftException.Factory(RetryCount, latestException);
    }
}

```