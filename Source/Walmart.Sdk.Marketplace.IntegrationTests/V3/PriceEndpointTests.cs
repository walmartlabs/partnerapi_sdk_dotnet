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
    using Xunit;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using System.Threading.Tasks;

    public class PriceEndpointTests : BaseIntegrationTest
    {
        private readonly Marketplace.V3.Api.PriceEndpoint priceApi;

        public PriceEndpointTests()
        {
            var config = new Marketplace.ClientConfig("test", "test-key");
            var apiClient = new Marketplace.ApiClient(config);
            apiClient.SimulationEnabled = true;
            priceApi = new Marketplace.V3.Api.PriceEndpoint(apiClient);
        }

        [Fact]
        public async Task UpdatePricesInBulk()
        {
            var stream = GetRequestStub("V3.requestStub.priceBulkUpdate");
            var result = await priceApi.UpdateBulkPrices(stream);
            Assert.IsType<FeedAcknowledgement>(result);
            Assert.NotEmpty(result.FeedId);
        }

        [Fact]
        public async Task UpdatePrice()
        {
            var stream = GetRequestStub("V3.requestStub.priceBulkUpdate");
            var result = await priceApi.UpdatePrice(stream);
            Assert.IsType<ItemPriceResponse>(result);
            Assert.NotEmpty(result.Message);
            Assert.NotEmpty(result.Sku);
        }
    }
}
