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

namespace Walmart.Sdk.Marketplace.E2ETests.V2
{
    using Xunit;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Payload.Feed;
    using System.Threading.Tasks;

    public class FeedEndpointTests : BaseE2ETest
    {
        private readonly FeedEndpoint feedApi;

        public FeedEndpointTests()
        {
            feedApi = new FeedEndpoint(client);
        }

        [Fact]
        public async Task CanRetrieveListOfFeeds()
        {
            var feeds = await feedApi.GetAllFeedStatuses();
            Assert.IsType<FeedRecordResponse>(feeds);
            Assert.NotEmpty(feeds.Results);
            foreach (var feed in feeds.Results)
            {
                Assert.True(feed.FeedId.Length > 0);
            }
        }

        [Fact]
        public async Task CanCheckFeedStatus()
        {
            var feeds = await feedApi.GetAllFeedStatuses();
            Assert.IsType<FeedRecordResponse>(feeds);
            Assert.NotEmpty(feeds.Results);
            var feedId = feeds.Results[0].FeedId;
            var result = await feedApi.GetFeedStatus(feedId, true);
            Assert.IsType<PartnerFeedResponse>(result);
            Assert.True(result.FeedId == feedId);
        }
    }
}
