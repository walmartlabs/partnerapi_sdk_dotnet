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

namespace Walmart.Sdk.Base.Primitive
{
    public enum ServiceNameType
    {
        WalmartMarketplace = 1,
        DropShipVendorServices = 2,
        WarehouseSupplierServices = 3
    }

    public static class ServiceName
    {
        public static string GetName(ServiceNameType type)
        {
            switch (type)
            {
                case ServiceNameType.DropShipVendorServices:
                    return "Drop Ship Vendor Services";
                case ServiceNameType.WarehouseSupplierServices:
                    return "Warehouse Supplier Services";
                default:
                case ServiceNameType.WalmartMarketplace:
                    return "Walmart Marketplace";
            }
        }
    }
}
