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
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Walmart.Sdk.Base.Http;
using Walmart.Sdk.Base.Primitive;

namespace Walmart.Sdk.Base.Http
{
    public class Request: IRequest
    {
        private Primitive.Config.IRequestConfig config;
        public Primitive.Config.IRequestConfig Config { get { return config; } }
        public string EndpointUri { get; set; }
        public HttpRequestMessage HttpRequest { get; }
        public Dictionary<string, string> QueryParams { get; set; } = new Dictionary<string, string>();

        public HttpMethod Method
        {
            get { return HttpRequest.Method; }
            set { HttpRequest.Method = value; }
        }

        public Request(Primitive.Config.IRequestConfig cfg)
        {
            config = cfg;
            HttpRequest = new HttpRequestMessage();
        }

        public void AddMultipartContent(byte[] content)
        {
            var multipartContent = new MultipartFormDataContent
            {
                new ByteArrayContent(content)
            };
            HttpRequest.Content = multipartContent;
        }

        public void AddMultipartContent(System.IO.Stream contentStream)
        {
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StreamContent(contentStream));
            HttpRequest.Content = multipartContent;
        }

        public void AddPayload(string payload)
        {
            HttpRequest.Content = new StringContent((string)payload, Encoding.UTF8, GetContentType());
        }

        public void AddPayload(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                HttpRequest.Content = new StringContent(reader.ReadToEnd(), Encoding.UTF8, GetContentType());
            }
        }

        public string BuildQueryParams()
        {
            var list = new List<string>();
            foreach (var param in this.QueryParams)
            {
                if (param.Value != null) {
                    list.Add(param.Key + "=" + param.Value);    
                }
            }
            if (list.Count > 0)
            {
                return "?" + string.Join("&", list);
            }
            return "";
        }

        public void FinalizePreparation()
        {
            HttpRequest.Headers.Add("User-Agent", config.UserAgent.Replace(" ", "_"));
            HttpRequest.RequestUri = new Uri(config.BaseUrl + EndpointUri + BuildQueryParams());
            // call to genereate walmart headers should be done when RequestUri already defined
            // we need it's value to generate signature header
            AddWalmartHeaders();
        }

        private void AddWalmartHeaders()
        {
            string timestamp = Util.DigitalSignature.GetCurrentTimestamp();
            string signature = GetSignature(timestamp);

            var creds = config.Credentials;
            HttpRequest.Headers.Add("WM_SEC.AUTH_SIGNATURE", signature);
            HttpRequest.Headers.Add("WM_SEC.TIMESTAMP", timestamp);
            HttpRequest.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", config.ChannelType);
            HttpRequest.Headers.Add("WM_CONSUMER.ID", creds.ConsumerId);
            HttpRequest.Headers.Add("WM_SVC.NAME", config.ServiceName);
            HttpRequest.Headers.Add("WM_QOS.CORRELATION_ID", Util.DigitalSignature.GetCorrelationId());

            HttpRequest.Headers.Add("Accept", GetContentType());
        }

        public string GetContentType()
        {
            switch (config.ApiFormat)
            {
                case Primitive.ApiFormat.JSON:
                    return "application/json";
                default:
                case Primitive.ApiFormat.XML:
                    return "application/xml";
            }
        }

        private string GetSignature(string timestamp)
        {
            if (config.Credentials is null)
            {
                throw new Base.Exception.InitException("Configuration is not initialized with Merchant Credentials!");
            }

            var creds = config.Credentials;
            var requestUri = HttpRequest.RequestUri.ToString();
            var httpMethod = HttpRequest.Method.Method.ToUpper();
            // Construct the string to sign
            string stringToSign = string.Join("\n", new List<string>() {
                creds.ConsumerId,
                requestUri,
                httpMethod,
                timestamp
            }) + "\n"; // extra newline symbol required for valid signature

            try
            {
                return Util.DigitalSignature.SignData(stringToSign, creds.PrivateKey);
            }
            catch (System.Exception ex)
            {
                //pop up this to the user of SDK 
                throw Base.Exception.SignatureException.Factory(creds.ConsumerId, requestUri, httpMethod, ex);
            }
        }
    }
}
