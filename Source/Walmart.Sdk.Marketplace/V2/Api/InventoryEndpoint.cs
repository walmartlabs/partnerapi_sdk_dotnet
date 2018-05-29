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
    using System.Threading.Tasks;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Marketplace.V2.Payload.Inventory;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;

    public class InventoryEndpoint: BaseEndpoint
    {
        private FeedEndpoint feedApi;

        public InventoryEndpoint(IEndpointClient client) : base(client)
        {
            feedApi = new FeedEndpoint(apiClient);
            payloadFactory = new Payload.PayloadFactory();
        }

        public async Task<Inventory> GetInventory(string merchantSku)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v2/inventory";
            request.QueryParams.Add("sku", merchantSku);

            var response = await client.GetAsync(request);
            Inventory result = await ProcessResponse<Inventory>(response);
            return result;
        }

        private Base.Http.Request PrepareUpdateInventoryRequest(string merchantSku, string content)
        {
            if (string.IsNullOrEmpty(merchantSku))
            {
                throw new Base.Exception.InvalidValueException("Sku can't be empty for inventory update!");
            }

            var request = CreateRequest();
            request.EndpointUri = "/v2/inventory";

            request.AddPayload(content);
            request.QueryParams.Add("sku", merchantSku);
            return request;
        }

        public async Task<Inventory> UpdateInventory(Inventory inventory)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            // enforce XML serializer here, API doesn't support JSON in requests
            var content = payloadFactory.GetSerializer(ApiFormat.XML).Serialize<Inventory>(inventory);
            var request = PrepareUpdateInventoryRequest(inventory.Sku, content);

            var response = await client.PutAsync(request);
            var result = await ProcessResponse<Inventory>(response);
            return result;
        }

        public async Task<Inventory> UpdateInventory(string merchantSku, System.IO.Stream stream)
        {
            // avoiding deadlock if client execute this method synchronously
            await new ContextRemover();

            var content = new StreamReader(stream).ReadToEnd();
            var request = PrepareUpdateInventoryRequest(merchantSku, content);

            var response = await client.PutAsync(request);
            var result = await ProcessResponse<Inventory>(response);
            return result;
        }
        
        public async Task<Inventory> UpdateInventory(string merchantSku, string bodyContent)
        {
            // avoiding deadlock if client execute this method synchronously
            await new ContextRemover();

            var request = PrepareUpdateInventoryRequest(merchantSku, bodyContent);

            var response = await client.PutAsync(request);
            var result = await ProcessResponse<Inventory>(response);
            return result;
        }

        public async Task<FeedAcknowledgement> UpdateBulkInventory(System.IO.Stream stream)
        {
            return await feedApi.UploadFeed(stream, V2.Payload.FeedType.inventory);
        }
    }
}
