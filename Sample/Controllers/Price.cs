/**
 * Created Date: Wed Mar 01 2017
 * Author: kshah0@walmartlabs.com
 * 
 * This software is distributed free of cost.
 * Walmart assumes no responsibility for its support.
 * Copyright (c) 2017 Walmart Corporation
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Walmart.Sdk.Base.Serialization;
using Walmart.Sdk.Marketplac.Sample.Base;
using Walmart.Sdk.Marketplace.Sample.Controllers;
using Walmart.Sdk.Marketplace.Sample.QuizParams;
using Walmart.Sdk.Marketplace.V2.Payload.Feed;
using Walmart.Sdk.Marketplace.V3.Api;

namespace Walmart.Sdk.Marketplace.Sample.Controllers
{
    public class Price: BaseController, IController
    {
        public string Header { get; } = "Price Endpoint";

        protected Marketplace.V2.Api.PriceEndpoint EndpointV2;
        protected Marketplace.V3.Api.PriceEndpoint EndpointV3;

        public Price(ApiClient client) : base(client)
        {
            EndpointV2 = new V2.Api.PriceEndpoint(Client);
            EndpointV3 = new V3.Api.PriceEndpoint(Client);
        }

        protected override void CreateMenuDefinition()
        {

            MainOption = new MenuOption("c", Header);
            Menu = new List<MenuOption>()
            {
                new MenuOption("a", "Update price"),
                //new MenuOption("b", "Update promotional price"),
                new MenuOption("b", "Update bulk price")
                
            };
        }

        public override Command GetCommandByName(string ch)
        {
            var ds = Path.DirectorySeparatorChar;
            var absolutePath = Directory.GetCurrentDirectory() + ds + "resources" + ds + "requestExamples" + ds;
            var version = Settings.SelectedApiVersion == ApiVersion.V2 ? "v2" : "v3";
            if (ch == "a")
            {
                if (Settings.SelectedApiVersion == ApiVersion.V2)
                {
                    return new Command(this.UpdatePrice, new List<IParam>()
                    {
                        new StringParam("sku", "Item Sku"),
                        new StringParam("currency", "Price Currency", "USD"),
                        new PriceParam("price", "Price for item")
                    });
                }
                else
                {
                    return new Command(this.UpdatePrice, new List<IParam>()
                    {
                        new PathParam("path", "Path to the feed content", absolutePath + version + ds + "priceFeed.xml")
                    });
                }
                
            }
            /*else if (ch == "b")
            {
                return new Command(this.UpdatePromotionalPrice, new List<IParam>()
                {
                    new StringParam("path", "Path to the feed content", absolutePath + version + ds + "promoFeed.xml")
                });
            }*/
            // "b"
            return new Command(this.UpdateBulkPrice, new List<IParam>()
            {
                new PathParam("path", "Path to the feed content", absolutePath + version + ds + "priceBulkFeed.xml")
            });
            
        }

        public string UpdatePrice(Dictionary<string, object> args)
        {
            if (Settings.SelectedApiVersion == ApiVersion.V2)
                return UpdatePriceV2(args);
            
            return UpdatePriceV3(args);
        }

        public string UpdatePriceV2(Dictionary<string, object> args)
        {
            var sku = (string) args["sku"];
            var currency = (string) args["currency"];
            var price = (double) args["price"];
            var taskV2 = EndpointV2.UpdatePrice(sku, currency, price);
            return GetResult<ItemPriceResponse, V2.Api.Exception.ApiException>(taskV2);
        }

        public string UpdatePriceV3(Dictionary<string, object> args)
        {
            var path = (string) args["path"];
            var task = EndpointV3.UpdatePrice(File.OpenRead(path));
            return GetResult<V3.Payload.Feed.ItemPriceResponse, V2.Api.Exception.ApiException>(task);
        }

        /*public string UpdatePromotionalPrice(Dictionary<string, object> args)
        {
            return "";
        }*/

        public string UpdateBulkPrice(Dictionary<string, object> args)
        {
            var path = (string)args["path"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.UpdateBulkPrices(File.OpenRead(path));
                return GetResult<FeedAcknowledgement, V2.Api.Exception.ApiException>(taskV2);
            }

            var taskV3 = EndpointV3.UpdateBulkPrices(File.OpenRead(path));
            return GetResult<V3.Payload.Feed.FeedAcknowledgement, V3.Api.Exception.ApiException>(taskV3);
        }
    }
}