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
    using System.Threading.Tasks;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using Xunit;

    public class PromotionEndpointTests : BaseE2ETest
    {
        private readonly PromotionEndpoint promotionApi;

        public PromotionEndpointTests()
        {
            promotionApi = new PromotionEndpoint(client);
        }

        [Fact]
        public async Task UpdatingPromotionsInBulk()
        {
            using (var stream = LoadRequestStub("V2.requestStub.promotionFeed"))
            {
                var result = await promotionApi.UpdatePromotionPricesInBulk(stream);
                Assert.IsType<FeedAcknowledgement>(result);
                Assert.True(result.FeedId.Length > 0);
            }
        }
    }
}
