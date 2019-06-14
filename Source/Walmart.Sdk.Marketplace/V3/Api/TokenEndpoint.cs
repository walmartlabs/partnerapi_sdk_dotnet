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
    using System.Threading.Tasks;
    using Walmart.Sdk.Base.Primitive;
    using Walmart.Sdk.Marketplace.V3.Payload.Token;

    public class TokenEndpoint : BaseEndpoint
    {
        public TokenEndpoint(ApiClient client) : base(client)
        {
            payloadFactory = new V3.Payload.PayloadFactory();
        }

        public async Task<TokenFeedResponse> GetToken()
        {
            // to avoid deadlock if this method is executed synchronously
            await new ContextRemover();

            var request = CreateRequest();

            request.EndpointUri = "/v3/token";

            request.HttpRequest.Content = new StringContent("grant_type=client_credentials");
            request.HttpRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.PostAsync(request);
            var result = await ProcessResponse<TokenFeedResponse>(response);
            return result;
        }

    }
}
