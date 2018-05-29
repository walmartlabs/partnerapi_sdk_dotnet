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

namespace Walmart.Sdk.Marketplace.V3.Api
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.IO;
    using System.Threading.Tasks;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using Walmart.Sdk.Marketplace.V3.Payload.Promotion;
    using Walmart.Sdk.Marketplace.V3.Payload;

    public class PromotionEndpoint : Base.Primitive.BaseEndpoint
    {
        protected FeedEndpoint feedApi;
        public PromotionEndpoint(Base.Primitive.IEndpointClient apiClient) : base(apiClient)
        {
            feedApi = new FeedEndpoint(apiClient);
            payloadFactory = new V3.Payload.PayloadFactory();
        }
        public async Task<ServiceResponse> GetPromotionPrice(string merchantSku)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = string.Format("/v3/promo/sku/{0}", merchantSku);
            var response = await client.GetAsync(request);

            ServiceResponse result = await ProcessResponse<ServiceResponse>(response);
            return result;
        }

        public async Task<ItemPriceResponse> UpdatePromotionPrice(Stream stream)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v3/price?promo=true";
            var payload = new StreamReader(stream).ReadToEnd();
            request.HttpRequest.Content = new StringContent(payload);
            //set the headers to avoid broken responses because of encodding=utf-8
            request.HttpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.GetContentType());

            var response = await client.PutAsync(request);
            var result = await ProcessResponse<ItemPriceResponse>(response);
            return result;
        }

        public async Task<FeedAcknowledgement> UpdateBulkPromotions(Stream stream)
        {
            return await feedApi.UploadFeed(stream, FeedType.promo);
        }
    }
}
