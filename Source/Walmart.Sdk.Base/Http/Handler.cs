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

using System.Net.Http;
using System.Threading.Tasks;
using Walmart.Sdk.Base.Http.Fetcher;
using Walmart.Sdk.Base.Primitive.Config;

namespace Walmart.Sdk.Base.Http
{
    public class Handler: IHandler
    {
        public static IFetcherFactory FetcherFactory = new Http.Fetcher.FetcherFactory();

        private IHttpConfig config;

        public IFetcher Fetcher { get; private set; }

        public Retry.IRetryPolicy RetryPolicy { get; set; }

        private bool simulationEnabled = false;
        public bool SimulationEnabled
        {
            get { return simulationEnabled; }
            set
            {
                simulationEnabled = value;
                Fetcher = FetcherFactory.CreateFetcher(simulationEnabled, config);
            }
        }

        public Handler(IHttpConfig apiConfig)
        {
            config = apiConfig;
            SimulationEnabled = false;
            RetryPolicy = new Retry.LuckyMePolicy();
        }

        private Task<IResponse> ExecuteAsync(IRequest request)
        {
            return RetryPolicy.GetResponse(Fetcher, request);
        }

        public Task<IResponse> GetAsync(IRequest request)
        {
            request.Method = HttpMethod.Get;
            return ExecuteAsync(request);
        }

        public Task<IResponse> PostAsync(IRequest request)
        {
            request.Method = HttpMethod.Post;
            return ExecuteAsync(request);
        }

        public Task<IResponse> PutAsync(IRequest request)
        {
            request.Method = HttpMethod.Put;
            return ExecuteAsync(request);
        }

        public Task<IResponse> DeleteAsync(IRequest request)
        {
            request.Method = HttpMethod.Delete;
            return ExecuteAsync(request);
        }
    }
}
