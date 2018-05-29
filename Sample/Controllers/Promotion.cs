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
using System.ComponentModel;
using System.IO;
using System;
using System.Net.Http;
using Walmart.Sdk.Base.Serialization;
using Walmart.Sdk.Marketplac.Sample.Base;
using Walmart.Sdk.Marketplace.Sample.Controllers;
using Walmart.Sdk.Marketplace.Sample.QuizParams;
using Walmart.Sdk.Marketplace.V2.Payload.Feed;
using Walmart.Sdk.Marketplace.V3.Api;

namespace Walmart.Sdk.Marketplace.Sample.Controllers
{
    public class Promotion: BaseController, IController
    {
        public string Header { get; } = "Promotion Endpoint";

        protected Marketplace.V2.Api.PromotionEndpoint EndpointV2;
        protected Marketplace.V3.Api.PromotionEndpoint EndpointV3;

        public Promotion(ApiClient client) : base(client)
        {
            EndpointV2 = new V2.Api.PromotionEndpoint(client);
            EndpointV3 = new V3.Api.PromotionEndpoint(client);
        }

        protected override void CreateMenuDefinition()
        {
            MainOption = new MenuOption("d", Header);
            Menu = new List<MenuOption>()
            {
                new MenuOption("a","Update bulk promotion"),
                new MenuOption("b","Update single promotion"),
                new MenuOption("c","View promotions")
            };
        }

        public override Command GetCommandByName(string ch)
        {
            var ds = Path.DirectorySeparatorChar;
            var absolutePath = Directory.GetCurrentDirectory() + ds + "resources" + ds + "requestExamples" + ds;
            var version = Settings.SelectedApiVersion == ApiVersion.V2 ? "v2" : "v3";
            if (ch == "a")
            {
                return new Command(this.UpdateBulkPromotions, new List<IParam>()
                {
                    new PathParam("path", "Path to the feed content", absolutePath + version + ds + "promoFeed.xml") 
                });
            }
            else if (ch == "b")
            {
                return new Command(this.UpdatePromotion, new List<IParam>()
                {
                    new StringParam("sku", "Item Sku"),                    
                    new DateParam("effectiveDate", "The date when promotion starts", DateTime.UtcNow.AddDays(30)),
                    new DateParam("expirationDate", "The date when promotion ends", DateTime.UtcNow.AddDays(33)),
                    new StringParam("price", "Price for promotion of this sku"),
                    new PathParam("path", "Path to the feed content", absolutePath + version + ds + "promotionUpdate.xml") 
                });
            }
            else
            {
                return new Command(this.ViewPromotion, new List<IParam>()
                {
                    new StringParam("sku", "Item Sku")
                });
            }
        }

        public string UpdateBulkPromotions(Dictionary<string, object> args)
        {
            var path = (string)args["path"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.UpdatePromotionPricesInBulk(File.OpenRead(path));
                return GetResult<FeedAcknowledgement, V2.Api.Exception.ApiException>(taskV2);
            }

            var taskV3 = EndpointV3.UpdateBulkPromotions(File.OpenRead(path));
            return GetResult<V3.Payload.Feed.FeedAcknowledgement, V3.Api.Exception.ApiException>(taskV3);
        }

        public string UpdatePromotion(Dictionary<string, object> args)
        {
            var sku = (string)args["sku"];
            var effectiveDate = (DateTime)args["effectiveDate"];
            var expirationDate = (DateTime)args["expirationDate"];
            var price = (string)args["price"];
            var path = (string)args["path"];
            var content = File.ReadAllText(path);
            content = content
                .Replace("{{sku}}", sku)
                .Replace("{{effectiveDate}}", effectiveDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"))
                .Replace("{{expirationDate}}", expirationDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"))
                .Replace("{{price}}", price);
            var task = EndpointV3.UpdatePromotionPrice(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)));
            return GetResult<V3.Payload.Feed.ItemPriceResponse, V3.Api.Exception.ApiException>(task);

        }

        public string ViewPromotion(Dictionary<string, object> args)
        {
            var sku = (string)args["sku"];
            var task = EndpointV3.GetPromotionPrice(sku);
            return GetResult<V3.Payload.Promotion.ServiceResponse, V3.Api.Exception.ApiException>(task);
        }
       


    }
}
