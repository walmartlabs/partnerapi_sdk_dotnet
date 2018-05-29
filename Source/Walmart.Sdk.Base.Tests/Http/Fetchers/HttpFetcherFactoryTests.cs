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

namespace Walmart.Sdk.Base.Test.Http
{
    using Xunit;

    public class HttpFetcherFactoryTests
    {
        [Fact]
        public void FetcherCreatedOnlyOnce()
        {
            var config = new Primitive.BaseConfig("test", "test");
            config.BaseUrl = "http://www.test.com";
            config.RequestTimeoutMs = 1000;

            var fetcherFactory = new Base.Http.Fetcher.FetcherFactory();
            var fetcher = fetcherFactory.CreateFetcher(false, config);
            var sameFetcher = fetcherFactory.CreateFetcher(false, config);

            Assert.Same(fetcher, sameFetcher);

            var fetcherFactory2 = new Base.Http.Fetcher.FetcherFactory();
            var fakeFetcher = fetcherFactory2.CreateFetcher(true, config);
            var sameFakeFetcher = fetcherFactory2.CreateFetcher(true, config);

            Assert.Same(fakeFetcher, sameFakeFetcher);
        }
    }
}
