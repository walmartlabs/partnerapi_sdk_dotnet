***
# NOTICE:
 
## This repository has been archived and is not supported.
 
[![No Maintenance Intended](http://unmaintained.tech/badge.svg)](http://unmaintained.tech/)
***

# Walmart Partner API SDK for .NET

----------

The Walmart Partner API SDK for .NET allows you to build applications that easily integrates with the Walmart Marketplace API

This is a .NET SDK (written in C#) containing convenience libraries and examples of their usage for e-commerce merchants to conduct business on Walmart's marketplace platform.

Walmart's marketplace provides several secure HTTP REST end-points for merchants to submit their items for listing on Walmart's marketplace - Walmart.com. There are also secure HTTP REST end-points for updating prices and inventories, and retrieve order and update their status.

This .NET package demonstrates how to invoke the HTTP REST end-points using C#. It does not include examples in other programming languages such as Java, Perl, Python and PHP which also can be used to invoke the same HTTP REST end-points.
The package consists of a zip file containing a .Net Standard 1.3 Library and .Net Core 2.0 (or .NET Framework v4.6) project.

In order to invoke secure HTTP REST end-points, your company must be a registered seller on Walmart.com marketplace. Registration will provide you with a consumer ID and a private key to make secure calls.

Or you can use the "mock" end-points to get an idea of how the real end-points work. This can help you to start and test your integration even before you complete your registration.

## Target Frameworks

* .NET Framework 4.6
* .NET Standard 1.3

## Sample Application

You can find documentation [here](Sample/README.md).

### Docker Image for Sample Application

This repository provides a Dockerfile to build an image for sample application. Before you start, make sure Docker is installed and running ([instructions](https://docs.docker.com/install/)).

This project supply Dockerfile to build a Docker image with Sample application. To build it on your local machine run

```bash
docker build -t <image-name> .
```

The following command starts a container

```bash
docker run -it -v <folder-with-creds>:/app/settings <image-name>
```

Example:

Check out the source code of this repo in any folder, for example `/Users/test/WalmartMarketplaceClient`, then execute
```bash
cd /Users/test/WalmartMarketplaceClient/
docker build -t partnerapi-app .
```
You need to place your API creds as "credentials.json" file in the folder `/Users/test/WalmartMarketplaceClient/settings`.
 You can read more about this file [here](Sample/README.md)

Execute the following command to start Sample application 
```bash
docker run -v $(pwd)/settings:/app/settings -it partnerapi-app 
```

## Code Snippets

You can find list of examples [here](Docs/Snippets/README.md)

## Usage

### Installation

This package is distributed as a source code, you need to manually build it and link library from your project. NuGet repository will be available soon, stay tuned!

## Build

To build  `dotnet build Source/`

### To run tests:

Unit & Integration tests - `scripts/run_mp_fast_tests.cmd`

E2E tests - `scripts/run_mp_e2e_tests.cmd`

## API Coverage

| Object | Operation | V2/V3 | SDK Class -> Method | Tests V2/V3 |
| --- | --- | --- | --- | --- |
| Feeds      | [Get all feed statuses](https://developer.walmart.com/#/apicenter/marketPlace/latest#getAllFeedStatuses)               | Yes/Yes | FeedEndpoint.GetAllFeedStatuses          | No / No           |
|            | [Get a feedItems status](https://developer.walmart.com/#/apicenter/marketPlace/latest#getAFeedItemsStatus)             | Yes/Yes | FeedEndpoint.GetFeedStatus  | Yes/Yes           |
|            | [Get a feed status](https://developer.walmart.com/#/apicenter/marketPlace/latest#getAFeedStatus)                       | Yes/Yes | FeedEndpoint.GetFeedStatus | Yes/Yes           |
| Items      | [Get all items](https://developer.walmart.com/#/apicenter/marketPlace/latest#getAllItems)                              | Yes/Yes | ItemEndpoint.GetAllItems | Yes/Yes           |
|            | [Get an item](https://developer.walmart.com/#/apicenter/marketPlace/latest#getAnItem)                                  | Yes/Yes | ItemEndpoint.GetItem | Yes/Yes           |
|            | [Retire an item](https://developer.walmart.com/#/apicenter/marketPlace/latest#retireAnItem)                            | Yes/Yes | ItemEndpoint.RetireItem | Yes/Yes           |
|            | [Bulk create/update items](https://developer.walmart.com/#/apicenter/marketPlace/latest#bulkCreateUpdateItems)         | Yes/Yes | ItemEndpoint.BulkItemsUpdate | Yes/Yes           |
| Prices     | [Update a price](https://developer.walmart.com/#/apicenter/marketPlace/latest#updateAPrice)                            | Yes/Yes | PriceEndpoint.UpdatePrice | Yes/Yes           |
|            | [Update bulk prices](https://developer.walmart.com/#/apicenter/marketPlace/latest#updateBulkPrices)                    | Yes/Yes | PriceEndpoint.UpdateBulkPrices | Yes/Yes           |
|            | [Set up CPA SKU Opt-in/Opt-out](https://developer.walmart.com/#/apicenter/marketPlace/v3#setCPASKUopt)                 | NA/Yes | PriceEndpoint.OptInOutBulkCpaSku   | NA/No      |
|            | [Set up CPA SKU All](https://developer.walmart.com/#/apicenter/marketPlace/v3#setCPASKUoptAll)                         | NA/Yes | PriceEndpoint.SetupAllCpaSku | NA/No |
| Promotions | [Update Bulk Promotional Prices](https://developer.walmart.com/#/apicenter/marketPlace/v3#updateBulkPromotionalPrices) | Yes/Yes | PromotionEndpoint.UpdatePromotionPricesInBulk | Yes/Yes |
|            | [Promotional Prices](https://developer.walmart.com/#/apicenter/marketPlace/v3#getPromotionalPrices) | NA/Yes | PromotionEndpoint.GetPromotionPrice | NA/Yes |
|            | [Update a Promotional Price](https://developer.walmart.com/#/apicenter/marketPlace/v3#updateAPromotionalPrice) | NA/Yes | Promotion.Endpoint.UpdatePromotionPrice | NA/Yes |
| Orders     | [Get all released orders](https://developer.walmart.com/#/apicenter/marketPlace/latest#orderOverview)       | Yes/Yes | OrderEndpoint.GetAllReleasedOrders | No / No           |
|            | [Get all orders](https://developer.walmart.com/#/apicenter/marketPlace/latest#getAllOrders)                | Yes/Yes | OrderEndpoint.GetAllOrders | No/No           |
|            | [Get an order](https://developer.walmart.com/#/apicenter/marketPlace/latest#getAnOrder)                  | Yes/Yes | OrderEndpoint.GetOrderById | No/No           |
|            | [Acknowledge purchase order](https://developer.walmart.com/#/apicenter/marketPlace/latest#acknowledgingOrders)    | Yes/Yes | OrderEndpoint.AckOrder | No/No               |
|            | [Cancel order lines](https://developer.walmart.com/#/apicenter/marketPlace/latest#cancellingOrderLines)            | Yes/Yes | OrderEndpoint.CancelOrderLines | No/No               |
|            | [Refund order lines](https://developer.walmart.com/#/apicenter/marketPlace/latest#refundingOrderLines)            | Yes/Yes | OrderEndpoint.RefundOrderLines | No/No               |
|            | [Shipping notification/updates](https://developer.walmart.com/#/apicenter/marketPlace/latest#shippingNotificationsUpdates) | Yes/Yes | OrderEndpoint.ShippingUpdates | No/No               |
| Inventory  | [Get inventory for an item](https://developer.walmart.com/#/apicenter/marketPlace/latest#getInventoryForAnItem)     | Yes/NA | Inventory.GetInventory | Yes/NA           |
|            | [Update inventory for an item](https://developer.walmart.com/#/apicenter/marketPlace/latest#updateInventoryForAnItem)  | Yes/NA | Inventory.UpdateInventory | Yes/NA           |
|            | [Bulk update inventory](https://developer.walmart.com/#/apicenter/marketPlace/latest#bulkUpdateInventory)         | Yes/Yes | Inventory.UpdateBulkInventory | Yes/Yes           |

## Disclaimer

This software is distributed free of cost.

 * Walmart assumes no responsibility for its support
 * Copyright (c) 2018 Walmart Corporation
