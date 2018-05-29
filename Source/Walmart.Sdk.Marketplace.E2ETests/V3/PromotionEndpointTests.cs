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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.IO;
    using System.Xml.Serialization;
    using Xunit;
    using Walmart.Sdk.Marketplace.V3.Api;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using Walmart.Sdk.Marketplace.V3.Payload.Promotion;

    public class PromotionEndpointTests : BaseE2ETest
    {
        private readonly PromotionEndpoint promotionApi;
        private readonly ItemEndpoint itemApi;
        private readonly XmlSerializer xmlSerializer;

        public PromotionEndpointTests()
        {
            promotionApi = new PromotionEndpoint(client);
            itemApi = new ItemEndpoint(client);
            xmlSerializer = new XmlSerializer(typeof(ServiceResponse));
        }

        [Fact]
        public async Task UpdatingPromotionsInBulk()
        {
            using (var stream = LoadRequestStub("V3.requestStub.promotionFeed"))
            {
                var result = await promotionApi.UpdateBulkPromotions(stream);
                Assert.IsType<FeedAcknowledgement>(result);
                Assert.True(result.FeedId.Length > 0);
            }
        }

        private Stream GetRequestContentForPromotionUpdate(string sku, string type, string processMode)
        {
            var content = new StreamReader(LoadRequestStub("V3.requestStub.updatePromotion")).ReadToEnd();
            var date1 = DateTime.UtcNow.AddMonths(2);
            //.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            var date2 = date1.AddDays(7);
            var date3 = date1.AddDays(10);
            var date4 = date1.AddDays(20);
            content = content
                .Replace("{{sku}}", sku)
                .Replace("{{type}}", type)
                .Replace("{{mode}}", processMode)
                .Replace("{{date1}}",date1.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"))
                .Replace("{{date2}}", date2.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"))
                .Replace("{{date3}}", date3.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"))
                .Replace("{{date4}}", date4.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));
            var test = content;
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        [Fact(Skip = "The Partner API has parsing errors for Not found response")]
        public async Task CanUpdatePromotionForSpecificSku()
        {
            var oneItemList = await itemApi.GetAllItems(1, 20);
            var sku = oneItemList.ItemResponse[0].Sku;
            //Delete the current promotions if there is any
            //var result = await promotionApi.GetPromotionPrice(sku);
            //Assert.IsType<ServiceResponse>(result);
            //StringWriter textWriter = new StringWriter();
            //xmlSerializer.Serialize(textWriter, result);
            //var str = textWriter.ToString();
            //var payload = GetPayloadForDelete(str, sku);

            using (var stream = GetRequestContentForPromotionUpdate(sku, "REDUCED", "DELETE"))
            {
                var result = await promotionApi.UpdatePromotionPrice(stream);
                Assert.IsType<ItemPriceResponse>(result);
                Assert.True(result.Mart.Length > 0);
                Assert.True(result.Sku == sku);
                Assert.True(result.Message.Length > 0);

            }
        }

        private string GetPayloadForDelete(string text, string sku)
        {
            var str = text.Replace("ns2:","");
            var first = str.IndexOf("<pricing effectiveDate");
            if (first == -1)
                return "";
            else
            {
                var last = str.LastIndexOf("</pricing>") + "</pricing>".Length;
                var temp = str.Substring(first, last - first);
                string sPattern = "[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}";
                MatchCollection matches = Regex.Matches(temp, sPattern);
                string payload = "";
                for (int i = 0; i < matches.Count; i++) {
                    payload = temp.Replace("promoId=\"" + matches[i].ToString() + "\"", "processMode=\"DELETE\"");
                    temp = payload;

                }
                string prefix = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?>\r\n<Price xmlns=\"http://walmart.com/\">\r\n<itemIdentifier>\r\n<sku>"
                    + sku + "</sku>\r\n</itemIdentifier>\r\n<pricingList>\r\n";
                string suffix = "</pricingList>\r\n</Price>";
                return prefix + payload + suffix;
            }
            

        }

        [Fact (Skip = "The Partner API has parsing errors for Not found response")]
        public async Task GetPromotionForSingleSku()
        {
            var oneItemList = await itemApi.GetAllItems(1, 10);
            var sku = oneItemList.ItemResponse[0].Sku;
            var result = await promotionApi.GetPromotionPrice(sku);
            Assert.IsType<ServiceResponse>(result);
            Assert.True(result.Status.Equals(Status.OK));

            StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, result);
            var str = textWriter.ToString();
            var payload = GetPayloadForDelete(str, sku);
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
            var result1 = await promotionApi.UpdatePromotionPrice(stream);
            Assert.IsType<ItemPriceResponse>(result1);

        }
    }
}
