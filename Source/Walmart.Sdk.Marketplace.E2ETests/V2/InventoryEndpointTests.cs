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

namespace Walmart.Sdk.Marketplace.E2ETests.V2
{
    using System.IO;
    using System.Threading.Tasks;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using Walmart.Sdk.Marketplace.V2.Payload.Inventory;
    using Xunit;

    public class InventoryEndpointTests : BaseE2ETest
    {
        const string TEST_SKU = "NETSDK_TEST";

        private readonly InventoryEndpoint inventoryApi;

        public InventoryEndpointTests()
        {
            inventoryApi = new InventoryEndpoint(client);
        }

        [Fact]
        public async Task GetInventory()
        {
            var inventory = await inventoryApi.GetInventory(TEST_SKU);
            Assert.True(inventory.Sku.Length > 0);
            Assert.True(inventory.Quantity.Amount >= 0);
        }

        [Fact]
        public async Task UpdateInventoryWithStream()
        {
            var inventoryPayload = LoadRequestStub("V2.requestStub.inventoryUpdate");
            var update = await inventoryApi.UpdateInventory(TEST_SKU, inventoryPayload);
            Assert.IsType<Inventory>(update);
            Assert.True(update.Sku.Length > 0);
            Assert.True(update.Quantity.Amount >= 0);
        }

        [Fact]
        public async Task UpdateInventoryWithString()
        {
            var inventoryPayload = LoadRequestStub("V2.requestStub.inventoryUpdate");
            var content = (new StreamReader(inventoryPayload)).ReadToEnd();
            var update = await inventoryApi.UpdateInventory(TEST_SKU, content);
            Assert.True(update.Sku.Length > 0);
            Assert.True(update.Quantity.Amount >= 0);
        }

        [Fact]
        public async Task InventoryBulkUpdate()
        {
            var inventoryPayload = LoadRequestStub("V2.requestStub.inventoryBulkUpdate");
            var result = await inventoryApi.UpdateBulkInventory(inventoryPayload);
            Assert.IsType<FeedAcknowledgement>(result);
            Assert.True(result.FeedId.Length > 0);
        }
    }
}