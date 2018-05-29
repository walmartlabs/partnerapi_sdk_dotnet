# Order List Snippet 

Here is an example of how to get a list of all orders using SDK.
You can check [SDK Initialization]() to see an example on how to configure SDK

```csharp 

    // (!) this block is using async call

    ApiClient client = ... // check SDK Init snippet
    var ordersEndpoint = new Walmart.Sdk.Marketplace.V2.Api.OrderEndpoint(client);

    var filter = new OrderFilter();
    filter.CreatedEndDate = DateTime.Now;
    filter.CreatedStartDate = new DateTime(DateTime.Now.Year - 2, DateTime.Now.Month, 1);
    filter.ToExpectedShipDate = DateTime.Now;
    filter.FromExpectedShipDate = new DateTime(DateTime.Now.Year - 2, DateTime.Now.Month, 1);
    filter.CustomerOrderId = "<order-id>";
    filter.PurchaseOrderId = "<purchase-id>";
    filter.Status = OrderLineStatusValueType.Shipped;
    filter.Limit = 20;

    try
    {
        var orderList = await ordersEndpoint.GetAllOrders(filter);
        foreach (var order in firstPage.Elements.Orders)
        {
            Console.WriteLine("OrderId: {0}", PurchaseOrderId);
        }
    }
    catch (V2.Api.Exception.ApiException apiEx)
    {
        // process error from API
        // you can see list of possible errors here
        // https://developer.walmart.com/#/apicenter/marketPlace/latest#errors
    }
    
```