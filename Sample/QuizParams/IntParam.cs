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
using Walmart.Sdk.Marketplace.Sample.QuizParams;

namespace Walmart.Sdk.Marketplace.Sample.QuizParams
{
    class IntParam: IParam
    {
        public string Name { get; private set; }
        public string Title { get; private set; }
        public bool DefaultSelected = false;
        public int Default { get; private set; }

        public IntParam(string name, string title, int defaultValue = 10)
        {
            Name = name;
            Title = title;
            Default = defaultValue;
        }

        public object GetValue()
        {
            var defaultValue = " [" + Default + "]";
            ConsoleWriter.WriteLine(String.Format("Enter value for {0}{1}", Title, defaultValue));
            int value = -1;
            try
            {
                var rawInput = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(rawInput))
                    value = Int32.Parse(rawInput);
            }
            catch (Exception e)
            {
                
                value = Default;
                DefaultSelected = true;
            }
            
            if (value < 0)
            {
                value = Default;
                DefaultSelected = true;
            }

            return value;
        }
    }
}
