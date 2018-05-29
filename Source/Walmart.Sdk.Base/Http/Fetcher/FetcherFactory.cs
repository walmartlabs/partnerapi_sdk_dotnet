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

namespace Walmart.Sdk.Base.Http.Fetcher
{
    // in this class we are making sure 
    // we only creating one HttpClient and LocalClient 
    // for each instance of HttpHandler
    public class FetcherFactory: IFetcherFactory
    {
        private LocalFetcher localFetcher;
        private HttpFetcher httpFetcher;

        public IFetcher CreateFetcher(bool fakeFetcher, Primitive.Config.IHttpConfig config)
        {
            if (fakeFetcher)
            {
                if (localFetcher is null)
                    localFetcher = new LocalFetcher(config);
                return localFetcher;
            }
            else
            {
                if (httpFetcher is null)
                {
                    var client = new HttpClientWrapper();
                    httpFetcher = new HttpFetcher(config, client);
                }
                return httpFetcher;
            }
        }
    }
}
