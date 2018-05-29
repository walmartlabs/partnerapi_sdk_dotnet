

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


using System.Collections.Generic;
using System.IO;
using Walmart.Sdk.Marketplace.V2.Payload.Item;
using Walmart.Sdk.Marketplac.Sample.Base;
using Walmart.Sdk.Marketplace.V3.Payload.Feed;
using Walmart.Sdk.Marketplace.Sample.QuizParams;

namespace Walmart.Sdk.Marketplace.Sample.Controllers
{
    public class Item : BaseController, IController
    {
        public string Header { get; } = "Item Endpoint";

        protected V2.Api.ItemEndpoint EndpointV2;
        protected V3.Api.ItemEndpoint EndpointV3;

        public Item(ApiClient client) : base(client)
        {
            EndpointV2 = new V2.Api.ItemEndpoint(Client);
            EndpointV3 = new V3.Api.ItemEndpoint(Client);
        }

        protected override void CreateMenuDefinition()
        {
            MainOption = new MenuOption("b", Header);
            Menu = new List<MenuOption>()
            {
                new MenuOption("a", "List Items"),
                new MenuOption("b", "View Item details"),
                new MenuOption("c", "Create Item"),
                new MenuOption("d", "Update Item"),
                new MenuOption("e", "Retire Item")
            };
        }

        public override Command GetCommandByName(string ch)
        {
            var ds = Path.DirectorySeparatorChar;
            var version = Settings.SelectedApiVersion == ApiVersion.V2 ? "v2" : "v3";
            var absolutePath = Directory.GetCurrentDirectory() + ds + "resources" + ds + "requestExamples" + ds + version + ds;
            if (ch == "a")
            {
                return new Command(this.ListItems, new List<IParam>()
                {
                    new IntParam("limit", "Number of Items to return", 10),
                    new IntParam("offset", "Number of Items to skip", 0)
                });
            }
            else if (ch == "b")
            {
                return new Command(this.ViewItemDetails, new List<IParam>()
                {
                    new StringParam("sku", "Item SKU")
                });
            }
            else if (ch == "c")
            {
                return new Command(this.CreateItem, new List<IParam>()
                {
                    new PathParam("path", "full path to Item file", absolutePath + "itemFeed.xml")
                });
            }
            else if (ch == "d")
            {
                return new Command(this.CreateItem, new List<IParam>()
                {
                    new PathParam("path", "full path to Item file", absolutePath + "itemFeed.xml")
                });
            }

            return new Command(this.RetireItem, new List<IParam>()
            {
                new StringParam("sku", "Item Sku")
            });
        }

        public string RetireItem(Dictionary<string, object> args)
        {
            var sku = (string) args["sku"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.RetireItem(sku);
                return GetResult<V2.Payload.Item.ItemRetireResponse, V2.Api.Exception.ApiException>(taskV2);
            }

            var taskV3 = EndpointV3.RetireItem(sku);
            return GetResult<V3.Payload.Feed.ItemRetireResponse, V3.Api.Exception.ApiException>(taskV3);
        }

        public string CreateItem(Dictionary<string, object> args)
        {
            var path = (string) args["path"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.BulkItemsUpdate(File.OpenRead(path));
                return GetResult<V2.Payload.Feed.FeedAcknowledgement, V3.Api.Exception.ApiException>(taskV2);
            }

            var taskV3 = EndpointV3.BulkItemsUpdate(File.OpenRead(path));
            return GetResult<V3.Payload.Feed.FeedAcknowledgement, V3.Api.Exception.ApiException>(taskV3);
        }

        public string ListItems(Dictionary<string, object> args)
        {
            var limit = (int) args["limit"];
            var offset = (int) args["offset"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
                return GetResult<ItemViewList, V2.Api.Exception.ApiException>(EndpointV2.GetAllItems(limit, offset));
            return GetResult<V3.Payload.Feed.ItemResponses, V3.Api.Exception.ApiException>(EndpointV3.GetAllItems(limit, offset));
        }

        public string ViewItemDetails(Dictionary<string, object> args)
        {
            var sku = (string) args["sku"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var v2Task = EndpointV2.GetItem(sku);
                return GetResult<ItemView, V2.Api.Exception.ApiException>(v2Task);
            }

            var v3Task = EndpointV3.GetItem(sku);
            return GetResult<ItemResponse, V3.Api.Exception.ApiException>(v3Task);

        }
        
    }
}