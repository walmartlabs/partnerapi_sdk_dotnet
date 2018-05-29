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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using Walmart.Sdk.Base.Http.Exception;
using Walmart.Sdk.Base.Primitive;
using System.Net.Sockets;
using System.Net;

namespace Walmart.Sdk.Base.Http.Fetcher
{
    public class HttpFetcher : BaseFetcher
    {
        private IHttpClient client;

        public HttpFetcher(Primitive.Config.IHttpConfig config, IHttpClient httpClient) : base(config)
        {
            client = httpClient;
            client.BaseAddress = new Uri(config.BaseUrl);
            client.Timeout = TimeSpan.FromMilliseconds(config.RequestTimeoutMs);
        }

        override public async Task<IResponse> ExecuteAsync(IRequest request) 
        {
            if (request.EndpointUri == "") {
                throw new Base.Exception.InvalidValueException("Empty URI for the endpoint!");
            }

            // we add them when all data is in place
            request.FinalizePreparation();
            try
            {
                var response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    // 503 Service Unavailable
                    throw new GatewayException("Service is unavailable, gateway connection error");
                }

                if (response.StatusCode == (HttpStatusCode)429)
                {
                    // 429 Too many requests
                    throw new ThrottleException("HTTP request was throttled");
                }

                return response;
            }
            catch (System.Exception ex) when (IsNetworkError(ex) || ex is TaskCanceledException)
            {
                // unable to connect to API because of network/timeout
                throw new ConnectionException("Network error while connecting to the API", ex);
            }
        }

        private static bool IsNetworkError(System.Exception ex)
        {
            if (ex is SocketException)
                return true;
            if (ex.InnerException != null)
                return IsNetworkError(ex.InnerException);
            return false;
        }
    }
}
