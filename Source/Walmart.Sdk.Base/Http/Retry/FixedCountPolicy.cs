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
    public class FixedCountPolicy: BasePolicy
    {
        public const int DEFAULT_DELAY = 5000;
        public const int DEFAULT_RETRY_COUNT = 3;

        public int RetryCount { get; private set; } = DEFAULT_RETRY_COUNT;
        public int DelayMs;

        public FixedCountPolicy(int attempts, int delayMs = DEFAULT_DELAY)
        {
            if (attempts < 1)
            {
                throw new Base.Exception.InitException("Retry count should be more than 0");
            }
            RetryCount = attempts;
            DelayMs = delayMs < 0 ? DEFAULT_DELAY : delayMs;
        }

        public override async Task<IResponse> GetResponse(Http.Fetcher.IFetcher fetcher, IRequest request)
        {
            for (var i=0; i<RetryCount; i++)
            {
                // give it a try
                if (await ExecuteOnce(fetcher, request))
                    return response;

                // give it a break before another retry
                if (DelayMs > 0)
                    await Task.Delay(DelayMs);
            }
            throw Http.Exception.NoRetriesLeftException.Factory(RetryCount, latestException);
        }
    }
}
