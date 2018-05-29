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

namespace Walmart.Sdk.Marketplace.IntegrationTests.V2
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using Walmart.Sdk.Marketplace.V2.Payload.LagTime;

    public class LagTimeEndpointTests : BaseIntegrationTest
    {
        private LagTimeEndpoint lagtimeApi;
        
        public LagTimeEndpointTests()
        {
            var config = new ClientConfig("test", "test-key");
            var apiClient = new ApiClient(config);
            apiClient.SimulationEnabled = true;
            lagtimeApi = new LagTimeEndpoint(apiClient);
        }

        [Fact]
        public async Task UpdateLagTime()
        {
            var stream = GetRequestStub("V2.requestStub.updateLagTime");
            var result = await lagtimeApi.UpdateLagTime(stream);
            Assert.IsType<FeedAcknowledgement>(result);
        }

        [Fact]
        public async Task GetLagTime()
        {
            var result = await lagtimeApi.GetLagTime("test");
            Assert.IsType<LagTime>(result);           
        }
    }
}
