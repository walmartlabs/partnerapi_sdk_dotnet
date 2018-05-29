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
    using System.Threading.Tasks;
    using Walmart.Sdk.Marketplace.V2.Payload.Item;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;

    public class ItemEndpoint: BaseEndpoint
    {
        private FeedEndpoint feedApi;

        public ItemEndpoint(IEndpointClient apiClient) : base(apiClient)
        {
            feedApi = new FeedEndpoint(apiClient);
            payloadFactory = new V2.Payload.PayloadFactory();
        }

        public async Task<FeedAcknowledgement> BulkItemsUpdate(System.IO.Stream stream)
        {
            return await feedApi.UploadFeed(stream, V2.Payload.FeedType.item);
        }
        
        public async Task<ItemViewList> GetAllItems(int limit = 10, int offset = 0)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v2/items";

            if (limit > 0) request.QueryParams.Add("limit", limit.ToString());
            if (offset > 0) request.QueryParams.Add("offset", offset.ToString());

            var response = await client.GetAsync(request);
            ItemViewList result = await ProcessResponse<ItemViewList>(response);
            return result;
        }

        public async Task<ItemView> GetItem(string merchantSku)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = string.Format("/v2/items/{0}", merchantSku);

            var response = await client.GetAsync(request);
            ItemViewList result = await ProcessResponse<ItemViewList>(response);
            return result.Items[0];
        }

        public async Task<ItemRetireResponse> RetireItem(string merchantSku)
        {
            // avoiding deadlock if client execute this method synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = string.Format("/v2/items/{0}", merchantSku);

            var response = await client.DeleteAsync(request);
            ItemRetireResponse result = await ProcessResponse<ItemRetireResponse>(response);
            return result;
        }
    }
}
