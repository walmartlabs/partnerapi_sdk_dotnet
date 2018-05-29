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

/**
 * Created Date: Wed Mar 01 2017
 * Author: kshah0@walmartlabs.com
 * 
 * This software is distributed free of cost.
 * Walmart assumes no responsibility for its support.
 * Copyright (c) 2017 Walmart Corporation
 */

using System;

namespace Walmart.Sdk.Marketplace.Sample
{
    public enum ApiVersion
    {
        V2,
        V3
    }

    class Program
    {
        private static Application App;

        private static void Main(string[] args)
        {
            try
            {
                App = new Sample.Application();
                App.Run();
            }
            catch (Exception e)
            {
                ConsoleWriter.WriteLine(String.Format("Exiting with error >{0}<. Press any key...", e.Message));
                Console.ReadKey();
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }
    }
}