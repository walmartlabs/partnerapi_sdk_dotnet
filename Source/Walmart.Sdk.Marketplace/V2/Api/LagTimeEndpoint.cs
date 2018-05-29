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
    using System.Collections.Generic;
    using System.Text;
    using Walmart.Sdk.Base;
    using System.Threading.Tasks;
    using Walmart.Sdk.Marketplace.V2;
    using Walmart.Sdk.Marketplace.V2.Payload;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using Walmart.Sdk.Marketplace.V2.Payload.Promotion;
    using Walmart.Sdk.Marketplace.V2.Payload.LagTime;
    
    public class LagTimeEndpoint : Base.Primitive.BaseEndpoint
    {
        protected FeedEndpoint feedApi;

        public LagTimeEndpoint(Base.Primitive.IEndpointClient apiClient) : base(apiClient)
        {
            feedApi = new FeedEndpoint(apiClient);
            payloadFactory = new V2.Payload.PayloadFactory();
        }

        public async Task<LagTime> GetLagTime(string merchantSku)
        {
            var request = CreateRequest();
            request.EndpointUri = string.Format("/v2/lagtime?sku={0}", merchantSku);
            var response = await client.GetAsync(request);

            LagTime result = await ProcessResponse<LagTime>(response);

            return result;
        }

        public async Task<FeedAcknowledgement> UpdateLagTime(Stream stream)
        {
            return await feedApi.UploadFeed(stream, FeedType.lagtime);
        }
    }
}
