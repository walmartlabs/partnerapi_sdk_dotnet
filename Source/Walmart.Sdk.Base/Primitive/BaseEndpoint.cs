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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Walmart.Sdk.Base.Http;
using Walmart.Sdk.Base.Primitive.Config;
using Walmart.Sdk.Base.Serialization;

namespace Walmart.Sdk.Base.Primitive
{
    public class BaseEndpoint
    {
        protected Config.IEndpointConfig config;
        protected IPayloadFactory payloadFactory;
        protected IEndpointHttpHandler client;
        protected Base.Primitive.IEndpointClient apiClient;

        public object ApiException { get; private set; }

        public BaseEndpoint(IEndpointClient apiClient)
        {
            this.apiClient = apiClient;
            client = apiClient.GetHttpHandler();
            config = apiClient.GetEndpointConfig();
        }

        protected Request CreateRequest()
        {
            return new Request(config.GetRequestConfig());
        }

        public async Task<TPayload> ProcessResponse<TPayload>(IResponse response)
        {
            if (!response.IsSuccessful)
            {
                var rawErrors = await response.GetPayloadAsString();
                System.Exception ex;
                try
                {
                    ex = payloadFactory.CreateApiException(config.ApiFormat, rawErrors, response);
                }
                catch (System.Exception e)
                {
                    var innerEx = new System.Exception("Error response is " + rawErrors, e);
                    throw new Base.Exception.InvalidValueException("Invalid error response received. Unable to parse with error!", innerEx);
                }

                throw ex;
            }
            string content = await response.GetPayloadAsString();
            var serializer = payloadFactory.GetSerializer(config.ApiFormat);
            return serializer.Deserialize<TPayload>(content);
        }

        public ISerializer GetSerializer()
        {
            return payloadFactory.GetSerializer(config.ApiFormat);
        }
    }
}
