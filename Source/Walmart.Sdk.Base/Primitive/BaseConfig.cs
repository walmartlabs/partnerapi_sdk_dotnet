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

using System.Reflection;
using Walmart.Sdk.Base.Http;
using Walmart.Sdk.Base.Http.Retry;
using Walmart.Sdk.Base.Primitive;
using Walmart.Sdk.Base.Primitive.Config;

namespace Walmart.Sdk.Base.Primitive
{
    // Main configuration class, holds all global settings for SDK
    public class BaseConfig: IRequestConfig, IApiClientConfig
    {
        public Credentials Credentials { get; private set; }
        public string BaseUrl { get; set; } = "https://marketplace.walmartapis.com";
        virtual public string ServiceName { get; set; } = "";
        public string ChannelType { get; set; }
        public string UserAgent { get; set; }
        public ApiFormat PayloadFormat { get; set; } = ApiFormat.XML;
        // just another name for paylod format we also use it for http request
        // and in this case it helps to determine value of contet-type header, not payload
        public ApiFormat ApiFormat
        {
            get { return PayloadFormat; }
            set { PayloadFormat = value; }
        }
        public int RequestTimeoutMs { get; set; } = 100000; // in milliseconds

        public BaseConfig(string clientId, string clientSecret, string accessToken)
        {
            // generate sdk name from an assembly information
            var assembly = this.GetType().GetTypeInfo().Assembly;
            UserAgent = string.Format(".Net_{0}_v{1}_{2}", assembly.GetName().Name, assembly.GetName().Version.ToString(), clientId);

            // storing user credentials
            Credentials = new Credentials(clientId, clientSecret, accessToken);
        }

        public IRequestConfig GetRequestConfig() => this;
    }

    // TODO: only xml supported at the moment
    public enum ApiFormat
    {
        XML,
        JSON,
    }
    
    // Merchant Credentials
    public class Credentials
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string AccessToken { get; private set; }

        public Credentials(string clientId, string clientSecret, string accessToken)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            AccessToken = accessToken;
        }
    }
}