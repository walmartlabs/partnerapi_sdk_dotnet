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

namespace Walmart.Sdk.Marketplace.IntegrationTests.V3
{
    using Xunit;
    using Walmart.Sdk.Marketplace.V3.Payload;
    using Walmart.Sdk.Marketplace.V3.Payload.Feed;
    using System.Threading.Tasks;

    /// <summary>
    ///  Class for testing feedApi
    /// </summary>
    /// <remarks>
    /// Please update the test case below to test the API endpoint.чгтше
    /// </remarks>
    public class FeedEndpointTests: BaseIntegrationTest
    {
        private readonly Marketplace.V3.Api.FeedEndpoint feedApi;

        public FeedEndpointTests()
        {
            var config = new Marketplace.ClientConfig("test", "test-key");
            var apiClient = new Marketplace.ApiClient(config);
            apiClient.SimulationEnabled = true;
            feedApi = new Marketplace.V3.Api.FeedEndpoint(apiClient);
        }

        /// <summary>
        /// Test GetFeedStatus
        /// </summary>
        [Fact]
        public async Task FeedStatusReturnsResult()
        {
            PartnerFeedResponse result = await feedApi.GetFeedStatus("test");
            Assert.IsType<PartnerFeedResponse>(result);
            Assert.NotEmpty(result.FeedId);
        }

        [Fact]
        public async Task CanGetAllFeedStatuses()
        {
            FeedRecordResponse result = await feedApi.GetAllFeedStatuses();
            Assert.IsType<FeedRecordResponse>(result);
            Assert.NotEmpty(result.Results);
            Assert.True(result.TotalResults > 0);
        }

        [Fact]
        public async Task CanUploadItemFile()
        {
            var stream = GetRequestStub("V3.requestStub.itemFeed");
            var result = await feedApi.UploadFeed(stream, FeedType.item);
            Assert.IsType<FeedAcknowledgement>(result);
            Assert.NotEmpty(result.FeedId);
        }
    }
}
