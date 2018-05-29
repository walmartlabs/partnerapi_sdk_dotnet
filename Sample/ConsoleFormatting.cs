/**
 * Created Date: Wed Mar 01 2017
 * Author: kshah0@walmartlabs.com
 * 
 * This software is distributed free of cost.
 * Walmart assumes no responsibility for its support.
 * Copyright (c) 2017 Walmart Corporation
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sdk.Marketplace.Sample
{
    public class ConsoleFormatting
    {
        public static string Indent(int count)
        {
            return "".PadLeft(count);
        }
    }
}