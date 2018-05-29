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
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using Walmart.Sdk.Marketplace.V3.Payload;
    using Walmart.Sdk.Marketplace.V3.Payload.Cpa;
    using Walmart.Sdk.Base.Primitive;

    public class PriceEndpoint: Base.Primitive.BaseEndpoint
    {
        private FeedEndpoint feedApi;

        public PriceEndpoint(ApiClient client) : base(client)
        {
            feedApi = new FeedEndpoint(client);
            payloadFactory = new V3.Payload.PayloadFactory();
        }

        public async Task<FeedAcknowledgement> UpdateBulkPrices(Stream stream)
        {
            return await feedApi.UploadFeed(stream, FeedType.price);
        }

        public async Task<ItemPriceResponse> UpdatePrice(Stream stream)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v3/price";
            var payload = new StreamReader(stream).ReadToEnd();
            request.HttpRequest.Content = new StringContent(payload);
            // have to explicitly set this header for content, otherwise it also has encodding=utf-8
            // and it breaks response from API
            request.HttpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.GetContentType());

            var response = await client.PutAsync(request);
            
            var result = await ProcessResponse<ItemPriceResponse>(response);
            return result;
        }

        public async Task<FeedAcknowledgement> OptInOutBulkCpaSku(Stream stream)
        {
            return await feedApi.UploadFeed(stream, FeedType.CPT_SELLER_ELIGIBILITY);
        }

        public async Task<EnrollmentResponse> SetupAllCpaSku(bool enroll)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v3/cppreference";
            string content = JsonConvert.SerializeObject(new Payload.Cpa.Enrollment()
            {
                Enroll = enroll
            });
            request.HttpRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");
            request.HttpRequest.Headers.Add("Accept", "application/json");
            
            var response = await client.PostAsync(request);
            var responseContent = await response.GetPayloadAsString();
            var result = JsonConvert.DeserializeObject<EnrollmentResponse>(responseContent);
            return result;
        }
    }
}
