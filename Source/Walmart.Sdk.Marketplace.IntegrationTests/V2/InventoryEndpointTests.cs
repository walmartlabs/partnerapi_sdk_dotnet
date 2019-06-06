/**
Copyright (c) 2018-present, Walmart Inc.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

namespace Walmart.Sdk.Marketplace.IntegrationTests.V2
{
    using Xunit;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Base.Primitive.Config;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Payload;
    using Walmart.Sdk.Marketplace.V2.Payload.Inventory;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using System.IO;
    using System.Threading.Tasks;

    public class InventoryEndpointTests: BaseIntegrationTest
    {
        private readonly InventoryEndpoint inventoryApi;

        public InventoryEndpointTests()
        {
            var config = new Marketplace.ClientConfig("test", "test-key", "Test");
            var apiClient = new Marketplace.ApiClient(config);
            apiClient.SimulationEnabled = true;
            inventoryApi = new Marketplace.V2.Api.InventoryEndpoint(apiClient);
        }

        [Fact]
        public async Task CanGetInventoryForSpecificSku()
        {
            var result = await inventoryApi.GetInventory("test");

            Assert.IsType<Inventory>(result);
            Assert.NotEmpty(result.Sku);
            Assert.True(result.Quantity.Amount > 0);
        }

        [Fact]
        public async Task CanUpdateInventoryForSpecificSkuWithObject()
        {
            var inventory = new Inventory()
            {
                Sku = "test",
                Quantity = new Quantity()
                {
                    Amount = 100,
                    Unit = UnitOfMeasurement.EACH
                },
                FulfillmentLagTime = 1
            };

            var result = await inventoryApi.UpdateInventory(inventory);

            Assert.IsType<Inventory>(result);
            Assert.NotEmpty(result.Sku);
            Assert.True(result.Quantity.Amount > 0);
        }

        [Fact]
        public async Task CanUpdateInventoryForSpecificSkuWithStream()
        {
            var stream = GetRequestStub("V2.requestStub.inventoryUpdate");

            var result = await inventoryApi.UpdateInventory("test", stream);

            Assert.IsType<Inventory>(result);
            Assert.NotEmpty(result.Sku);
            Assert.True(result.Quantity.Amount > 0);
        }

        [Fact]
        public async Task CanUpdateInventoryForSpecificSkuWithString()
        {
            var stream = GetRequestStub("V2.requestStub.inventoryUpdate");
            var content = new StreamReader(stream).ReadToEnd();

            var result = await inventoryApi.UpdateInventory("test", content);

            Assert.IsType<Inventory>(result);
            Assert.NotEmpty(result.Sku);
            Assert.True(result.Quantity.Amount > 0);
        }

        [Fact]
        public async Task CanUpdateInventoryInBulk()
        {
            var stream = GetRequestStub("V2.requestStub.inventoryBulkUpdate");

            FeedAcknowledgement result = await inventoryApi.UpdateBulkInventory(stream);

            Assert.IsType<FeedAcknowledgement>(result);
            Assert.NotEmpty(result.FeedId);
        }
    }
}
