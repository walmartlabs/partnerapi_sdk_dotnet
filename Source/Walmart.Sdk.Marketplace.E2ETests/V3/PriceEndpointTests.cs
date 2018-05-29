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

namespace Walmart.Sdk.Marketplace.E2ETests.V3
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Walmart.Sdk.Marketplace.V3.Api;
    using Walmart.Sdk.Marketplace.V3.Payload.Cpa;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using Xunit;

    public class PriceEndpointTests : BaseE2ETest
    {
        private readonly PriceEndpoint priceApi;
        private readonly ItemEndpoint itemApi;

        public PriceEndpointTests()
        {
            priceApi = new PriceEndpoint(client);
            itemApi = new ItemEndpoint(client);
        }

        [Fact]
        public async Task UpdatingPricesInBulk()
        {
            using (var stream = LoadRequestStub("V3.requestStub.priceFeed"))
            {
                var result = await priceApi.UpdateBulkPrices(stream);
                Assert.IsType<FeedAcknowledgement>(result);
                Assert.True(result.FeedId.Length > 0);
            }
        }

        private Stream GetRequestContentForPriceUpdate(string sku, double price, string currency)
        {
            var content = new StreamReader(LoadRequestStub("V3.requestStub.updatePrice")).ReadToEnd();
            content = content
                .Replace("{{sku}}", sku)
                .Replace("{{price}}", price.ToString())
                .Replace("{{currency}}", currency);
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        [Fact]
        public async Task CanUpdatePriceForSpecificSku()
        {
            var sku = "NETSDK_TEST";
            using (var stream = GetRequestContentForPriceUpdate(sku, 400.00, "USD"))
            {
                var result = await priceApi.UpdatePrice(stream);
                Assert.IsType<ItemPriceResponse>(result);
                Assert.True(result.Sku == sku);
            }
        }

        [Fact]
        public async Task CanEnrollInCpaForAllSkus()
        {
            var result = await priceApi.SetupAllCpaSku(true);
            Assert.IsType<EnrollmentResponse>(result);
            Assert.True(result.EnrollStatus);
        }

        [Fact]
        public async Task CanUnEnrollFromCpaForAllSkus()
        {
            var result = await priceApi.SetupAllCpaSku(false);
            Assert.IsType<EnrollmentResponse>(result);
            Assert.False(result.EnrollStatus);
        }
    }
}
