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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Walmart.Sdk.Base.Http
{
    public class Logger : DelegatingHandler
    {
        private Primitive.LoggerContainer log;
        public Logger(HttpMessageHandler innerHandler) : base(innerHandler)
        {
            log = Primitive.LoggerContainer.GetInstance();
        }

        private async Task<string> GenerateRequestMessage(HttpRequestMessage request)
        {
            var sBuilder = new StringBuilder();
            var sWriter = new StringWriter(sBuilder);

            sWriter.WriteLine("Request:");
            sWriter.WriteLine(request.ToString());
            if (request.Content != null)
            {
                sWriter.WriteLine(await request.Content.ReadAsStringAsync());
            }
            sWriter.WriteLine();
            return sBuilder.ToString();
        }

        private async Task<string> GenerateResponseLog(HttpResponseMessage response)
        {
            var sBuilder = new StringBuilder();
            var sWriter = new StringWriter(sBuilder);

            sWriter.WriteLine("Response:");
            sWriter.WriteLine(response.ToString());
            if (response.Content != null)
            {
                sWriter.WriteLine(await response.Content.ReadAsStringAsync());
            }
            sWriter.WriteLine();
            return sBuilder.ToString();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (log.IsLevelEnabled(Primitive.LogLevel.DEBUG))
            {
                var debugMessage = await GenerateRequestMessage(request);
                log.Debug(debugMessage);
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (log.IsLevelEnabled(Primitive.LogLevel.DEBUG))
            {
                var debugMessage = await GenerateResponseLog(response);
                log.Debug(debugMessage);
            }

            return response;
        }
    }
}
