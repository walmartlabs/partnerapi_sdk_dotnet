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
    using System.Text;
    using Xunit;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using System.Threading.Tasks;

    public class PriceEndpointTests: BaseE2ETest
    {
        private readonly PriceEndpoint priceApi;
        private readonly ItemEndpoint itemApi;

        public PriceEndpointTests()
        {
            priceApi = new PriceEndpoint(client);
            itemApi = new ItemEndpoint(client);
        }

        private Stream GetRequestContentForPriceUpdate(string resourceName, string sku, double price, string currency)
        {
            var content = new StreamReader(LoadRequestStub(resourceName)).ReadToEnd();
            content = content
                .Replace("{{sku}}", sku)
                .Replace("{{price}}", price.ToString())
                .Replace("{{currency}}", currency);
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        [Fact]
        public async Task CanUpdatePriceForSpecificItem()
        {
            var amount = 99990.0;
            var currency = "USD";
            var sku = "NETSDK_TEST";

            var result = await priceApi.UpdatePrice(sku, currency, amount);
            Assert.IsType<ItemPriceResponse>(result);
            Assert.Equal(sku, result.Sku);
        }

        [Fact]
        public async Task CanUpdatePricesInBulk()
        {
            var amount = 400.0;
            var currency = "USD";
            var oneItemList = await itemApi.GetAllItems(1, 10);
            var sku = oneItemList.Items[0].Sku;
            var payloadStream = GetRequestContentForPriceUpdate("V2.requestStub.updatePrice", sku, amount, currency);
            var result = await priceApi.UpdateBulkPrices(payloadStream);
            Assert.IsType<FeedAcknowledgement>(result);
            Assert.True(result.FeedId.Length > 0);
        }
    }
}
