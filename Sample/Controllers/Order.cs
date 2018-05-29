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
using System.Threading.Tasks;
using Walmart.Sdk.Marketplac.Sample.Base;
using Walmart.Sdk.Marketplace.Sample.QuizParams;
using Walmart.Sdk.Marketplace.V2.Api.Request;
using Walmart.Sdk.Marketplace.V2.Payload.Feed;
using Walmart.Sdk.Marketplace.V2.Payload.Order;

namespace Walmart.Sdk.Marketplace.Sample.Controllers
{
    public class Order : BaseController, IController
    {
        public string Header { get; } = "Order Endpoint";

        protected Marketplace.V2.Api.OrderEndpoint EndpointV2;
        protected Marketplace.V3.Api.OrderEndpoint EndpointV3;

        public Order(ApiClient client) : base(client)
        {
            EndpointV2 = new V2.Api.OrderEndpoint(Client);
            EndpointV3 = new V3.Api.OrderEndpoint(Client);
        }

        protected override void CreateMenuDefinition()
        {
            MainOption = new MenuOption("e", Header);
            Menu = new List<MenuOption>()
            {
                new MenuOption("a", "List Orders"),
                new MenuOption("b", "List Released orders"),
                new MenuOption("c", "List Created/Released orders for specific date"),
                new MenuOption("d", "List Acknowledged orders"),
                new MenuOption("e", "List Acknowledged orders created on a specific date"),
                new MenuOption("f", "List Shipped/Refunded orders"),
                new MenuOption("g", "List Shipped orders created on a specific date"),
                new MenuOption("h", "List Cancelled orders"),
                new MenuOption("i", "List Cancelled orders created on a specific date"),

                new MenuOption("j", "List orders for Specific SKU"),
                new MenuOption("k", "List orders created after specific date"),
                new MenuOption("l", "List orders created before specific date"),
                new MenuOption("m", "List orders with expected ship date after specific date"),
                new MenuOption("n", "List orders to be shipped based on creation date"),
                new MenuOption("o", "View order detail"),
                new MenuOption("p", "Acknowledge order"),
                new MenuOption("q", "Ship order"),
                new MenuOption("r", "Cancel order"),
                new MenuOption("s", "Refund order")
            };
            
        }

        public override Command GetCommandByName(string ch)
        {
            var now = DateTime.Now;
            var ds = Path.DirectorySeparatorChar;
            var version = Settings.SelectedApiVersion == ApiVersion.V2 ? "v2" : "v3";
            var absolutePath = Directory.GetCurrentDirectory() + ds + "resources" + ds + "requestExamples" + ds + version + ds;
            if (ch == "a")
            {
                return new Command(this.ListOrders, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10)
                });
            }
            else if (ch == "b")
            {
                return new Command(this.ListReleasedOrders, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10)
                });
            }
            else if (ch == "c")
            {
                return new Command(this.ListCreatedOrdersByDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("createdStartDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day)),
                    new DateParam("createdEndDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day))
                });
            }
            else if (ch == "d")
            {
                return new Command(this.ListAckOrders, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10)
                });
            }
            else if (ch == "e")
            {
                return new Command(this.ListAckOrdersByDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("createdStartDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day)),
                    new DateParam("createdEndDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day))
                });
            }
            else if (ch == "f")
            {
                return new Command(this.ListShippedOrders, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10)
                });
            }
            else if (ch == "g")
            {
                return new Command(this.ListShippedOrdersByDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("createdStartDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day)),
                    new DateParam("createdEndDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day))
                });
            }
            else if (ch == "h")
            {
                return new Command(this.ListCancelledOrders, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10)
                });
            }
            else if (ch == "i")
            {
                return new Command(this.ListCancelledOrdersByDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("createdStartDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day)),
                    new DateParam("createdEndDate", "Return number of feeds",
                        new DateTime(now.Year, now.Month, now.Day))
                });
            }
            else if (ch == "j")
            {
                return new Command(this.ListOrdersForSku, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new StringParam("sku", "Sku of the Item")
                });
            }
            else if (ch == "k")
            {
                return new Command(this.ListOrdersAfterDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("createdStartDate", "Starting Date", new DateTime(now.Year, now.Month, now.Day)),
                });
            }
            else if (ch == "l")
            {
                return new Command(this.ListOrdersBeforeDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("createdEndDate", "Ending Date", new DateTime(now.Year, now.Month, now.Day)),
                });
            }
            else if (ch == "m")
            {
                return new Command(this.ListOrdersFromExpectedShippingDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("fromShippingDate", "From Ship Date", new DateTime(now.Year, now.Month, now.Day)),
                });
            }
            else if (ch == "n")
            {
                return new Command(this.ListOrdersToExpectedShippingDate, new List<IParam>()
                {
                    new StringParam("cursor", "Cursor value for next page"),
                    new IntParam("limit", "Number of items to return", 10),
                    new DateParam("toShippingDate", "To Ship Date", new DateTime(now.Year, now.Month, now.Day)),
                });
            }
            else if (ch == "o")
            {
                return new Command(this.ViewOrderDetails, new List<IParam>()
                {
                    new StringParam("orderId", "Purchase Order ID", "test")
                });
            }
            else if (ch == "p")
            {
                return new Command(this.AcknowledgeOrder, new List<IParam>()
                {
                    new StringParam("orderId", "Purchase Order ID", "test")
                });
            }
            else if (ch == "q")
            {
                return new Command(this.ShipOrder, new List<IParam>()
                {
                    new StringParam("orderId", "Purchase Order ID", "test"),
                    new PathParam("path", "Order Ship Feed File", absolutePath + "shippingOrder.xml")
                });
            }
            else if (ch == "r")
            {
                return new Command(this.CancelOrder, new List<IParam>()
                {
                    new StringParam("orderId", "Purchase Order ID", "test"),
                    new PathParam("path", "Order Cancel Feed File", absolutePath + "cancelOrder.xml")
                });
            }
            // if (ch == "s")
            return new Command(this.RefundOrder, new List<IParam>()
            {
                new StringParam("orderId", "Order ID", "test"),
                new PathParam("path", "Order Refund Feed File", absolutePath + "refundOrder.xml")
            });
        }

        public string ListOrders(Dictionary<string, object> args)
        {
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 5, endDate.Month, endDate.Day);

            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                Task<OrdersListType> taskV2;
                if (String.IsNullOrEmpty(cursor))
                {
                    var filterV2 = new V2.Api.Request.OrderFilter();
                    filterV2.CreatedEndDate = endDate;
                    filterV2.CreatedStartDate = startDate;
                    filterV2.Limit = limit;

                    taskV2 = EndpointV2.GetAllOrders(filterV2);
                }
                else
                {
                    taskV2 = EndpointV2.GetAllOrders(cursor);
                }

                return GetResult<OrdersListType, V2.Api.Exception.ApiException>(taskV2);
            }

            Task<V3.Payload.Order.OrdersListType> taskV3;
            if (String.IsNullOrEmpty(cursor))
            {
                var filterV3 = new V3.Api.Request.OrderFilter();
                filterV3.CreatedEndDate = endDate;
                filterV3.CreatedStartDate = startDate;
                filterV3.Limit = limit;
                taskV3 = EndpointV3.GetAllOrders(filterV3);
                return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
            }
            else
            {
                taskV3 = EndpointV3.GetAllOrders(cursor);
                return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
            }

        }

        private string GetOrdersForCursor(string cursor)
        {

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.GetAllOrders(cursor);
                return GetResult<OrdersListType, V2.Api.Exception.ApiException>(taskV2);
            }

            var taskV3 = EndpointV3.GetAllOrders(cursor);
            return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
        }

        private string GetOrdersByDateAndStatus(object status, DateTime startDate, DateTime endDate, int limit)
        {

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var statusV2 = (OrderLineStatusValueType) status;
                var filterV2 = new V2.Api.Request.OrderFilter();
                filterV2.CreatedEndDate = endDate;
                filterV2.CreatedStartDate = startDate;
                filterV2.Limit = limit;
                filterV2.Status = statusV2;

                var taskV2 = EndpointV2.GetAllOrders(filterV2);
                return GetResult<OrdersListType, V2.Api.Exception.ApiException>(taskV2);
            }

            var statusV3 = (V3.Payload.Order.OrderLineStatusValueType) status;
            var filterV3 = new V3.Api.Request.OrderFilter();
            filterV3.CreatedEndDate = endDate;
            filterV3.CreatedStartDate = startDate;
            filterV3.Limit = limit;
            filterV3.Status = statusV3;

            var taskV3 = EndpointV3.GetAllOrders(filterV3);
            return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
        }

        public string ListReleasedOrders(Dictionary<string, object> args)
        {
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 5, endDate.Month, endDate.Day);

            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Created, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Created, startDate, endDate,
                    limit);
            }
        }

        public string ListCreatedOrdersByDate(Dictionary<string, object> args)
        {
            var startDate = (DateTime) args["createdStartDate"];
            var endDate = (DateTime) args["createdEndDate"];
            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Created, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Created, startDate, endDate,
                    limit);
            }
        }

        public string ListAckOrders(Dictionary<string, object> args)
        {
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 5, endDate.Month, endDate.Day);

            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Acknowledged, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Acknowledged, startDate,
                    endDate, limit);
            }
        }

        public string ListAckOrdersByDate(Dictionary<string, object> args)
        {
            var startDate = (DateTime) args["createdStartDate"];
            var endDate = (DateTime) args["createdEndDate"];
            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Acknowledged, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Acknowledged, startDate,
                    endDate, limit);
            }
        }

        public string ListShippedOrders(Dictionary<string, object> args)
        {
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 5, endDate.Month, endDate.Day);

            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Shipped, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Shipped, startDate, endDate,
                    limit);
            }
        }

        public string ListShippedOrdersByDate(Dictionary<string, object> args)
        {
            var startDate = (DateTime) args["createdStartDate"];
            var endDate = (DateTime) args["createdEndDate"];
            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Shipped, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Shipped, startDate, endDate,
                    limit);
            }
        }

        public string ListCancelledOrders(Dictionary<string, object> args)
        {
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 5, endDate.Month, endDate.Day);

            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Cancelled, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Cancelled, startDate, endDate,
                    limit);
            }
        }

        public string ListCancelledOrdersByDate(Dictionary<string, object> args)
        {
            var startDate = (DateTime) args["createdStartDate"];
            var endDate = (DateTime) args["createdEndDate"];
            var limit = (int) args["limit"];
            var cursor = (string) args["cursor"];

            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                return GetOrdersByDateAndStatus(OrderLineStatusValueType.Cancelled, startDate, endDate, limit);
            }
            else
            {
                return GetOrdersByDateAndStatus(V3.Payload.Order.OrderLineStatusValueType.Cancelled, startDate, endDate,
                    limit);
            }
        }

        public string ListOrdersForSku(Dictionary<string, object> args)
        {
            var cursor = (string) args["cursor"];
            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            var now = DateTime.Now;
            var startDate = new DateTime(now.Year - 1, now.Month, now.Day);
            var sku = (string) args["sku"];
            var limit = (int) args["limit"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var statusV2 = OrderLineStatusValueType.Created;
                var filterV2 = new V2.Api.Request.OrderFilter();
                filterV2.Limit = limit;
                filterV2.CreatedStartDate = startDate;
                filterV2.Sku = sku;

                var taskV2 = EndpointV2.GetAllOrders(filterV2);
                return GetResult<OrdersListType, V2.Api.Exception.ApiException>(taskV2);
            }

            var statusV3 = V3.Payload.Order.OrderLineStatusValueType.Created;
            var filterV3 = new V3.Api.Request.OrderFilter();
            filterV3.Limit = limit;
            filterV3.CreatedStartDate = startDate;
            filterV3.Sku = sku;

            var taskV3 = EndpointV3.GetAllOrders(filterV3);
            return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
        }

        private string GetOrdersByDate(DateTime startDate, DateTime endDate, int limit)
        {

            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var filterV2 = new V2.Api.Request.OrderFilter();
                filterV2.CreatedEndDate = endDate;
                filterV2.CreatedStartDate = startDate;
                filterV2.Limit = limit;

                var taskV2 = EndpointV2.GetAllOrders(filterV2);
                return GetResult<OrdersListType, V2.Api.Exception.ApiException>(taskV2);
            }

            var filterV3 = new V3.Api.Request.OrderFilter();
            filterV3.CreatedEndDate = endDate;
            filterV3.CreatedStartDate = startDate;
            filterV3.Limit = limit;

            var taskV3 = EndpointV3.GetAllOrders(filterV3);
            return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
        }

        public string ListOrdersAfterDate(Dictionary<string, object> args)
        {
            var cursor = (string) args["cursor"];
            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            var limit = (int) args["limit"];
            var startDate = (DateTime) args["createdStartDate"];
            var endDate = DateTime.Now;
            return GetOrdersByDate(startDate, endDate, limit);
        }

        public string ListOrdersBeforeDate(Dictionary<string, object> args)
        {
            var cursor = (string) args["cursor"];
            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            var limit = (int) args["limit"];
            var endDate = (DateTime) args["createdEndDate"];
            var startDate = new DateTime(endDate.Year - 1, endDate.Month, endDate.Day);
            return GetOrdersByDate(startDate, endDate, limit);
        }

        public string ListOrdersFromExpectedShippingDate(Dictionary<string, object> args)
        {
            var cursor = (string) args["cursor"];
            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            var limit = (int) args["limit"];
            var fromShippingDate = (DateTime) args["fromShippingDate"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var filterV2 = new V2.Api.Request.OrderFilter()
                {
                    FromExpectedShipDate = fromShippingDate,
                    Limit = limit
                };

                var taskV2 = EndpointV2.GetAllOrders(filterV2);
                return GetResult<OrdersListType, V2.Api.Exception.ApiException>(taskV2);
            }

            var filterV3 = new V3.Api.Request.OrderFilter();

            filterV3.Limit = limit;
            filterV3.FromExpectedShipDate = fromShippingDate;
            var taskV3 = EndpointV3.GetAllOrders(filterV3);
            return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
        }

        public string ListOrdersToExpectedShippingDate(Dictionary<string, object> args)
        {
            var cursor = (string) args["cursor"];
            if (!String.IsNullOrEmpty(cursor))
            {
                return GetOrdersForCursor(cursor);
            }

            var limit = (int) args["limit"];
            var toShippingDate = (DateTime) args["toShippingDate"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var filterV2 = new V2.Api.Request.OrderFilter();
                filterV2.ToExpectedShipDate = toShippingDate;
                filterV2.Limit = limit;

                var taskV2 = EndpointV2.GetAllOrders(filterV2);
                return GetResult<OrdersListType, V2.Api.Exception.ApiException>(taskV2);
            }

            var filterV3 = new V3.Api.Request.OrderFilter();

            filterV3.Limit = limit;
            filterV3.ToExpectedShipDate = toShippingDate;
            var taskV3 = EndpointV3.GetAllOrders(filterV3);
            return GetResult<V3.Payload.Order.OrdersListType, V2.Api.Exception.ApiException>(taskV3);
        }

        public string ViewOrderDetails(Dictionary<string, object> args)
        {
            var orderId = (string) args["orderId"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.GetOrderById(orderId);
                return GetResult<V2.Payload.Order.Order, V2.Api.Exception.ApiException>(taskV2);
            }
            else
            {
                var taskV3 = EndpointV3.GetOrderById(orderId);
                return GetResult<V3.Payload.Order.Order, V3.Api.Exception.ApiException>(taskV3);
            }
        }

        public string AcknowledgeOrder(Dictionary<string, object> args)
        {
            var orderId = (string) args["orderId"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.AckOrder(orderId);
                return GetResult<V2.Payload.Order.Order, V3.Api.Exception.ApiException>(taskV2);
            }
            else
            {
                var taskV3 = EndpointV3.AckOrder(orderId);
                return GetResult<V3.Payload.Order.Order, V3.Api.Exception.ApiException>(taskV3);
            }
        }

        public string ShipOrder(Dictionary<string, object> args)
        {
            var path = (string) args["path"];
            var orderId = (string) args["orderId"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.ShippingUpdates(orderId, File.OpenRead(path));
                return GetResult<V2.Payload.Order.Order, V2.Api.Exception.ApiException>(taskV2);
            }
            else
            {
                var taskV3 = EndpointV3.ShippingUpdates(orderId, File.OpenRead(path));
                return GetResult<V3.Payload.Order.Order, V3.Api.Exception.ApiException>(taskV3);
            }
        }

        public string CancelOrder(Dictionary<string, object> args)
        {
            var path = (string)args["path"];
            var orderId = (string)args["orderId"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.CancelOrderLines(orderId, File.OpenRead(path));
                return GetResult<V2.Payload.Order.Order, V2.Api.Exception.ApiException>(taskV2);
            }
            else
            {
                var taskV3 = EndpointV3.CancelOrderLines(orderId, File.OpenRead(path));
                return GetResult<V3.Payload.Order.Order, V3.Api.Exception.ApiException>(taskV3);
            }
        }

        public string RefundOrder(Dictionary<string, object> args)
        {
            var path = (string)args["path"];
            var orderId = (string)args["orderId"];
            if (Settings.SelectedApiVersion == ApiVersion.V2)
            {
                var taskV2 = EndpointV2.RefundOrderLines(orderId, File.OpenRead(path));
                return GetResult<V2.Payload.Order.Order, V2.Api.Exception.ApiException>(taskV2);
            }
            else
            {
                var taskV3 = EndpointV3.RefundOrderLines(orderId, File.OpenRead(path));
                return GetResult<V3.Payload.Order.Order, V3.Api.Exception.ApiException>(taskV3);
            }
        }
    }
}