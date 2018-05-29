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
using System.Text;
using Walmart.Sdk.Marketplace.V2.Payload.Order;

namespace Walmart.Sdk.Marketplace.V2.Api.Request
{
    public class OrderFilter
    {
        public string Sku;
        public string CustomerOrderId;
        public string PurchaseOrderId;
        public OrderLineStatusValueType? Status;
        public DateTime? CreatedStartDate;
        public DateTime? CreatedEndDate;
        public DateTime? FromExpectedShipDate;
        public DateTime? ToExpectedShipDate;
        public int Limit = 20;

        public void FullfilRequest(Base.Http.Request request)
        {
            if (!String.IsNullOrEmpty(Sku)) request.QueryParams.Add("sku", Sku);
            if (!String.IsNullOrEmpty(CustomerOrderId)) request.QueryParams.Add("customerOrderId", CustomerOrderId);
            if (!String.IsNullOrEmpty(PurchaseOrderId)) request.QueryParams.Add("purchaseOrderId", PurchaseOrderId);
            if (Status != null) request.QueryParams.Add("status", Status.ToString());
            if (CreatedStartDate != null) request.QueryParams.Add("createdStartDate", CreatedStartDate?.ToString("yyyy-MM-dd"));
            if (CreatedEndDate != null) request.QueryParams.Add("createdEndDate", CreatedEndDate?.ToString("yyyy-MM-dd"));
            if (FromExpectedShipDate != null) request.QueryParams.Add("fromExpectedShipDate", FromExpectedShipDate?.ToString("yyyy-MM-dd"));
            if (ToExpectedShipDate != null) request.QueryParams.Add("toExpectedShipDate", ToExpectedShipDate?.ToString("yyyy-MM-dd"));

            if (Limit < 1) request.QueryParams.Add("limit", "1");
            else if (Limit > 200) request.QueryParams.Add("limit", "200");
            else request.QueryParams.Add("limit", Limit.ToString());
        }
    }
}
