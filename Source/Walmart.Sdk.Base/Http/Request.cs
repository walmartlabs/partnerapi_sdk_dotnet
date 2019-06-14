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
using Walmart.Sdk.Base.Util;

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
            HttpRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
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

            var creds = config.Credentials;

            if(!String.IsNullOrEmpty(creds.AccessToken))
                HttpRequest.Headers.Add("WM_SEC.ACCESS_TOKEN", creds.AccessToken);

            HttpRequest.Headers.Add("WM_SEC.TIMESTAMP", timestamp);
            HttpRequest.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", config.ChannelType);

            string base64String = Base64Converter.Base64Encode(String.Format("{0}:{1}", creds.ClientId, creds.ClientSecret));

            HttpRequest.Headers.Add("Authorization", "Basic " + base64String);
            HttpRequest.Headers.Add("WM_SVC.NAME", config.ServiceName);
            HttpRequest.Headers.Add("WM_QOS.CORRELATION_ID", DigitalSignature.GetCorrelationId());
            
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
    }
}
