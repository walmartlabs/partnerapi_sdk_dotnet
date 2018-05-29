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
using System.IO;
using System.Text;
using Walmart.Sdk.Marketplac.Sample.Base;
using Walmart.Sdk.Marketplace.Sample.QuizParams;
using Walmart.Sdk.Marketplace.V2.Payload.Feed;

namespace Walmart.Sdk.Marketplace.Sample.Controllers
{
    public class Feed: BaseController, IController
    {
        public string Header { get; } = "Feed Endpoint";

        protected Marketplace.V2.Api.FeedEndpoint EndpointV2;
        protected Marketplace.V3.Api.FeedEndpoint EndpointV3;

        public Feed(ApiClient client) : base(client)
        {
            EndpointV2 = new V2.Api.FeedEndpoint(Client);
            EndpointV3 = new V3.Api.FeedEndpoint(Client);
        }

        protected override void CreateMenuDefinition()
        {
            MainOption = new MenuOption("a", Header);
            Menu = new List<MenuOption>()
            {
                new MenuOption("a", "List Feeds"),
                new MenuOption("b", "View feed status summary"),
                new MenuOption("c", "View feed status details")
            };
            
            Commands = new Dictionary<string, Command>()
            {
                {
                    "a", new Command(this.ListFeeds, new List<IParam>()
                    {
                        new IntParam("limit", "Return number of feeds", 10),
                        new IntParam("offset", "Start from the following feed", 0)
                    })
                },
                {
                    "b", new Command(this.FeedStatus, new List<IParam>()
                    {
                        new StringParam("feedId", "Feed ID")
                    })
                },
                {
                    "c", new Command(this.FeedStatusWithDetails, new List<IParam>()
                    {
                        new StringParam("feedId", "Feed ID")
                    })
                }
            };
        }

        public string ListFeeds(Dictionary<string, object> args)
        {
            var limit = (int) args["limit"];
            var offset = (int) args["offset"];

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.GetAllFeedStatuses(offset, limit);
                return GetResult<V2.Payload.Feed.FeedRecordResponse, V2.Api.Exception.ApiException>(taskV2);
            }

            var taskV3 = EndpointV3.GetAllFeedStatuses(offset, limit);
            return GetResult<V3.Payload.Feed.FeedRecordResponse, V3.Api.Exception.ApiException>(taskV3);
        }

        public string GetFeedStatus(string feedId, bool includeDetails)
        {
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var v2Task = EndpointV2.GetFeedStatus(feedId, includeDetails);
                return GetResult<V2.Payload.Feed.PartnerFeedResponse, V2.Api.Exception.ApiException>(v2Task);
            }

            var v3Task = EndpointV3.GetFeedStatus(feedId, includeDetails);
            return GetResult<V3.Payload.Feed.PartnerFeedResponse, V3.Api.Exception.ApiException>(v3Task);
        }

        public string FeedStatus(Dictionary<string, object> args)
        {
            var feedId = (string)args["feedId"];
            return GetFeedStatus(feedId, false);
        }

        public string FeedStatusWithDetails(Dictionary<string, object> args)
        {
            var feedId = (string)args["feedId"];
            return GetFeedStatus(feedId, true);
        }
    }
}
