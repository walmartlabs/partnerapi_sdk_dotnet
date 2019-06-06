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

namespace Walmart.Sdk.Marketplace.IntegrationTests.V3
{
    using System.Threading.Tasks;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using Xunit;

    public class ItemEndpointTests: BaseIntegrationTest
    {
        private readonly Marketplace.V3.Api.ItemEndpoint itemApi;

        public ItemEndpointTests()
        {
            var config = new Marketplace.ClientConfig("test", "test-key", "Test");
            var apiClient = new Marketplace.ApiClient(config);
            apiClient.SimulationEnabled = true;
            itemApi = new Marketplace.V3.Api.ItemEndpoint(apiClient);
        }

        [Fact]
        public async Task UpdateItemsInBulk()
        {
            var stream = GetRequestStub("V3.requestStub.itemFeed");
            var result = await itemApi.BulkItemsUpdate(stream);
            Assert.IsType<FeedAcknowledgement>(result);
            Assert.NotEmpty(result.FeedId);
        }

        [Fact]
        public async Task GetOneSpecificItem()
        {
            var result = await itemApi.GetItem("test");
            Assert.IsType<ItemResponse>(result);
            Assert.True(result.Sku.Length > 0);
            Assert.True(result.ProductName.Length > 0);
            Assert.True(result.Gtin.Length > 0);
        }

        [Fact]
        public async Task RetireOneSpecificItem()
        {
            var result = await itemApi.RetireItem("test");
            Assert.True(result.Sku.Length > 0);
        }

        [Fact]
        public async Task GetListOfItems()
        {
            var result = await itemApi.GetAllItems();
            Assert.IsType<ItemResponses>(result);
            foreach (var item in result.ItemResponse)
            {
                Assert.IsType<ItemResponse>(item);
                Assert.True(item.Sku.Length > 0);
                Assert.True(item.ProductName.Length > 0);
                Assert.True(item.Gtin.Length > 0);
            }
        }
    }
}
