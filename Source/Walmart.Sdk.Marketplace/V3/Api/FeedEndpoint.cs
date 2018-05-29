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
    using System;
    using System.Threading.Tasks;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;

    public class FeedEndpoint: Base.Primitive.BaseEndpoint
    {
        public FeedEndpoint(Base.Primitive.IEndpointClient apiClient) : base(apiClient)
        {
            payloadFactory = new V3.Payload.PayloadFactory();
        }

        public async Task<PartnerFeedResponse> GetFeedStatus(string feedId, bool includeDetails = false, int offset = 0, int limit = 0)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = string.Format("/v3/feeds/{0}", feedId);

            // by default it's false and we don't need to send it
            if (includeDetails) request.QueryParams.Add("includeDetails", "true"); 
            if (offset > 0) request.QueryParams.Add("offset", offset.ToString());
            if (limit > 0) request.QueryParams.Add("limit", limit.ToString());

            var response = await client.GetAsync(request);
            var result = await ProcessResponse<PartnerFeedResponse>(response);
            return result;
        }

        public async Task<FeedRecordResponse> GetAllFeedStatuses(int offset = 0, int limit = 10)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v3/feeds";

            if (offset > 0) request.QueryParams.Add("offset", offset.ToString());
            if (limit > 0) request.QueryParams.Add("limit", limit.ToString());

            var response = await client.GetAsync(request);
            FeedRecordResponse result = await ProcessResponse<FeedRecordResponse>(response);

            return result;
        }

        public async Task<FeedAcknowledgement> UploadFeed(System.IO.Stream file, V3.Payload.FeedType feedType)
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();
            request.EndpointUri = "/v3/feeds";

            request.QueryParams.Add("feedType", feedType.ToString());
            request.AddMultipartContent(file);

            var response = await client.PostAsync(request);
            var result = await ProcessResponse<FeedAcknowledgement>(response);
            return result;
        }
    }
}
