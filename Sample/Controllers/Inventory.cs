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

using System.IO;
using System.Collections.Generic;
using Walmart.Sdk.Marketplac.Sample.Base;
using Walmart.Sdk.Marketplace.Sample.QuizParams;

namespace Walmart.Sdk.Marketplace.Sample.Controllers
{
    public class Inventory: BaseController, IController
    {
        public string Header { get; } = "Inventory Endpoint";

        protected Marketplace.V2.Api.InventoryEndpoint EndpointV2;
        protected Marketplace.V3.Api.InventoryEndpoint EndpointV3;

        public Inventory(ApiClient client) : base(client)
        {
            EndpointV2 = new V2.Api.InventoryEndpoint(Client);
            EndpointV3 = new V3.Api.InventoryEndpoint(Client);
        }

        protected override void CreateMenuDefinition()
        {
            MainOption = new MenuOption("f", Header);
            Menu = new List<MenuOption>()
            {
                new MenuOption("a", "Bulk Update"),
                new MenuOption("b", "Update for single Item"),
                new MenuOption("c", "View Item Inventory")
            };
        }

        public override Command GetCommandByName(string ch)
        {
            var ds = Path.DirectorySeparatorChar;
            var version = Settings.SelectedApiVersion == ApiVersion.V2 ? "v2" : "v3";
            var absolutePath = Directory.GetCurrentDirectory() + ds + "resources" + ds + "requestExamples" + ds + version + ds;
            if (ch == "a")
            {
                return new Command(this.BulkUpdate, new List<IParam>()
                {
                    new PathParam("path", "full path to inventory feed file", absolutePath + "inventoryBulkFeed.xml")
                });
            }
            else if (ch == "b")
            {
                return new Command(this.UpdateInventory, new List<IParam>()
                {
                    new PathParam("path", "full path to inventory file", absolutePath + "inventoryFeed.xml")
                });
            }

            return new Command(this.ViewInventory, new List<IParam>()
            {
                new StringParam("sku", "Item Sku")
            });
        }

        protected string BulkUpdate(Dictionary<string, object> args)
        {
            var path = (string) args["path"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var task = EndpointV2.UpdateBulkInventory(File.OpenRead(path));
                return GetResult<V2.Payload.Feed.FeedAcknowledgement, V2.Api.Exception.ApiException>(task);
            }
            else
            {
                var task = EndpointV3.UpdateBulkInventory(File.OpenRead(path));
                return GetResult<V3.Payload.Feed.FeedAcknowledgement, V3.Api.Exception.ApiException>(task);
            }
        }

        protected string UpdateInventory(Dictionary<string, object> args)
        {
            var path = (string)args["path"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var inventoryObj = GetObjectFromFile<V2.Payload.Inventory.Inventory>(path);
                var task = EndpointV2.UpdateInventory(inventoryObj);
                return GetResult<V2.Payload.Inventory.Inventory, V2.Api.Exception.ApiException>(task);
            }
            else
                return "This version is not yet supported. Please, switch to V2 in settings...";
        }

        protected string ViewInventory(Dictionary<string, object> args)
        {
            var sku = (string) args["sku"];
            var task = EndpointV2.GetInventory(sku);
            return GetResult<V2.Payload.Inventory.Inventory, V2.Api.Exception.ApiException>(task);
        }
    }
}