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
    using Walmart.Sdk.Marketplace.V3.Api;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using Walmart.Sdk.Marketplace.V3.Payload.Promotion;
    using System.Threading.Tasks;

    public class PromotionEndpointTests : BaseIntegrationTest
    {
        private readonly PromotionEndpoint promotionApi;

        public PromotionEndpointTests()
        {
            var config = new ClientConfig("test", "test-key");
            var apiClient = new ApiClient(config);
            apiClient.SimulationEnabled = true;
            promotionApi = new PromotionEndpoint(apiClient);
        }

        [Fact]
        public async Task UpdatePromotionsInBulk()
        {
            var stream = GetRequestStub("V3.requestStub.promotionFeed");
            var result = await promotionApi.UpdateBulkPromotions(stream);
            Assert.IsType<FeedAcknowledgement>(result);
            Assert.NotEmpty(result.FeedId);
        }

        [Fact]
        public async Task UpdateSinglePromotion()
        {
            var stream = GetRequestStub("V3.requestStub.updatePromotion");
            var result = await promotionApi.UpdatePromotionPrice(stream);
            Assert.IsType<ItemPriceResponse>(result);
            Assert.True(result.Mart.Length > 0);
            Assert.True(result.Sku.Length > 0);
            Assert.True(result.Message.Length > 0);
        }

        [Fact]
        public async Task GetPromotion()
        {
            var result = await promotionApi.GetPromotionPrice("test");
            Assert.IsType<ServiceResponse>(result);
            Assert.Equal(Status.OK, result.Status);
        }
    }
}
