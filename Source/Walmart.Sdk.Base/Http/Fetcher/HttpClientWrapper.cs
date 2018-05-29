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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sdk.Base.Http.Fetcher
{
    class HttpClientWrapper: IHttpClient
    {
        private HttpClient client;

        public HttpClientWrapper()
        {
            var loggingHandler = new Logger(new HttpClientHandler());
            client = new HttpClient(loggingHandler);
        }

        public TimeSpan Timeout {
            get { return client.Timeout; }
            set { client.Timeout = value; }
        }

        public Uri BaseAddress
        {
            get { return client.BaseAddress; }
            set { client.BaseAddress = value; }
        }

        public async Task<IResponse> SendAsync(IRequest request)
        {
            var result = await client.SendAsync(request.HttpRequest);
            return (IResponse)new Response(result);
        }
    }
}
