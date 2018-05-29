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
    using System.IO;
    using System.Threading.Tasks;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;

    public class ItemEndpoint: Base.Primitive.BaseEndpoint
    {
        protected FeedEndpoint feedApi;
        public ItemEndpoint(Base.Primitive.IEndpointClient apiClient) : base(apiClient)
        {
            feedApi = new FeedEndpoint(apiClient);
            payloadFactory = new V3.Payload.PayloadFactory();
        }

        public async Task<FeedAcknowledgement> BulkItemsUpdate(Stream stream)
        {
            return await feedApi.UploadFeed(stream, V3.Payload.FeedType.item);
        }

        public async Task<ItemResponses> GetAllItems(int limit = 10, int offset = 0)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v3/items";

            if (limit > 0) request.QueryParams.Add("limit", limit.ToString());
            if (offset > 0) request.QueryParams.Add("offset", offset.ToString());

            var response = await client.GetAsync(request);
            ItemResponses result = await ProcessResponse<ItemResponses>(response);
            return result;
        }

        public async Task<ItemResponse> GetItem(string merchantSku)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = string.Format("/v3/items/{0}", merchantSku);

            var response = await client.GetAsync(request);
            ItemResponses result = await ProcessResponse<ItemResponses>(response);
            return result.ItemResponse[0];
        }

        public async Task<ItemRetireResponse> RetireItem(string merchantSku)
        {
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = string.Format("/v3/items/{0}", merchantSku);

            var response = await client.DeleteAsync(request);
            ItemRetireResponse result = await ProcessResponse<ItemRetireResponse>(response);
            return result;
        }
    }
}
