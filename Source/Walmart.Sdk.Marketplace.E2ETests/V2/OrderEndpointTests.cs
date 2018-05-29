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
    using System;
    using System.Threading.Tasks;
    using Walmart.Sdk.Marketplace.V2.Api;
    using Walmart.Sdk.Marketplace.V2.Api.Request;
    using Walmart.Sdk.Marketplace.V2.Payload.Order;
    using Xunit;

    public class OrderEndpointTests: BaseE2ETest
    {
        private readonly OrderEndpoint orderApi;

        public OrderEndpointTests()
        {
            orderApi = new OrderEndpoint(client);
        }

        [Fact]
        public async Task RetrieveListOfFeeds()
        {
            var result = await orderApi.GetAllReleasedOrders(new DateTime(2017, 01, 01), new DateTime(2018, 04, 04), 20);
            Assert.IsType<OrdersListType>(result);
        }

        [Fact]
        public async Task ForReleasedOrdersGetFirstAndSecondPagesWithCursor()
        {
            var startDate = new DateTime(DateTime.Now.Year - 2, DateTime.Now.Month, 01);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            var limit = 1;

            var firstPage = await orderApi.GetAllReleasedOrders(startDate, endDate, limit);
            Assert.IsType<OrdersListType>(firstPage);
            Assert.Equal(limit, firstPage.Elements.Orders.Count);
            Assert.True(firstPage.Meta.NextCursor.Length > 0);
            var nextCursor = firstPage.Meta.NextCursor;

            var secondPage = await orderApi.GetAllReleasedOrders(nextCursor);
            Assert.IsType<OrdersListType>(secondPage);
            Assert.Equal(limit, secondPage.Elements.Orders.Count);
            Assert.True(secondPage.Meta.NextCursor.Length > 0);
        }

        [Fact]
        public async Task GetAllOrdersWithFiltering()
        {
            var filter = new OrderFilter();
            filter.CreatedEndDate = DateTime.Now;
            filter.CreatedStartDate = new DateTime(DateTime.Now.Year - 2, DateTime.Now.Month, 1);
            filter.Limit = 20;

            var firstPage = await orderApi.GetAllOrders(filter);
            Assert.IsType<OrdersListType>(firstPage);
            Assert.True(firstPage.Elements.Orders.Count > 0);
        }
    }
}
