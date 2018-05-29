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

namespace Walmart.Sdk.Marketplace.V2.Api
{
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Marketplace.V2.Payload;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;

    public class PriceEndpoint: BaseEndpoint
    {
        private FeedEndpoint feedApi;

        public PriceEndpoint(IEndpointClient apiClient) : base(apiClient)
        {
            feedApi = new FeedEndpoint(apiClient);
            payloadFactory = new V2.Payload.PayloadFactory();
        }

        public async Task<ItemPriceResponse> UpdatePrice(string sku, string currency, double price)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v2/prices";
            request.QueryParams.Add("sku", sku);
            request.QueryParams.Add("currency", currency);
            request.QueryParams.Add("price", price.ToString());
            request.HttpRequest.Content = new StringContent("");
            request.HttpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.GetContentType());

            var response = await client.PutAsync(request);
            var result = await ProcessResponse<ItemPriceResponse>(response);
            return result;
        }

        public async Task<FeedAcknowledgement> UpdateBulkPrices(Stream payload)
        {
            return await feedApi.UploadFeed(payload, FeedType.price);
        }
    }
}
