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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Walmart.Sdk.Base.Primitive;

namespace Walmart.Sdk.Base.Http
{
    public class Response: IResponse
    {
        private HttpResponseMessage originalResponse;
        public HttpStatusCode StatusCode
        {
            get { return originalResponse.StatusCode; }
        }
        public Response(HttpResponseMessage response)
        {
            originalResponse = response;
        }

        public HttpResponseMessage RawResponse { get { return originalResponse; } }

        public bool IsSuccessful { get { return originalResponse.IsSuccessStatusCode;  } }

        public async Task<string> GetPayloadAsString()
        {
            return await originalResponse.Content.ReadAsStringAsync();
        }
    }
}
