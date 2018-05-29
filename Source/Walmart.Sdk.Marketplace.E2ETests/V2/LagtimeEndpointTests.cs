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
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using System.IO;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Payload.LagTime;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using Walmart.Sdk.Marketplace.V2.Payload.Item;
    using Walmart.Sdk.Marketplace.V2.Payload.Promotion;

    public class LagTimeEndpointTests : BaseE2ETest
    {
        private LagTimeEndpoint lagtimeApi;
        private ItemEndpoint itemApi;

        public LagTimeEndpointTests()
        {
            lagtimeApi = new LagTimeEndpoint(client);
            itemApi = new ItemEndpoint(client);
        }

        [Fact(Skip = "do not have static sku for testing")]
        public async Task GetLagTime()
        {
            //var items = await itemApi.GetAllItems(1,10);
            //var sku = items.Items[0].Sku;
            var result = await lagtimeApi.GetLagTime("NETSDK_TEST");
            Assert.IsType<LagTime>(result);
        }

        public Stream GetRequestBody(string sku, string time)
        {
            var content = new StreamReader(LoadRequestStub("V2.requestStub.updateLagTime")).ReadToEnd();
            content = content
                .Replace("{{sku}}", sku)
                .Replace("{{time}}", time);
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        [Fact]
        public async Task UpdateLagTime()
        {
            var items = await itemApi.GetAllItems(1, 10);
            var sku = items.Items[0].Sku;
            using (var stream = GetRequestBody(sku, "0"))
            {
                var result = await lagtimeApi.UpdateLagTime(stream);
                Assert.IsType<FeedAcknowledgement>(result);
            }
        }
    }
}
