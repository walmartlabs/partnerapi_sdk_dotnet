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
using Walmart.Sdk.Base.Http;
using Walmart.Sdk.Base.Exception;

namespace Walmart.Sdk.Base.Http.Retry
{
    public abstract class BasePolicy: IRetryPolicy
    {
        public Http.Fetcher.IFetcher Fetcher;
        protected IResponse response;
        protected System.Exception latestException;

        protected async Task<bool> ExecuteOnce(Http.Fetcher.IFetcher fetcher, IRequest request)
        {
            try
            {
                response = await fetcher.ExecuteAsync(request);
                return true;
            }
            catch (Http.Exception.HttpException ex)
            {
                latestException = ex;
                return false;
            }
        }

        public abstract Task<IResponse> GetResponse(Http.Fetcher.IFetcher fetcher, IRequest request);
    }
}
