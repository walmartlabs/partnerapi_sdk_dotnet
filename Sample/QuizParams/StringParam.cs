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
using Org.BouncyCastle.Asn1.Crmf;

namespace Walmart.Sdk.Marketplace.Sample.QuizParams
{
    public class StringParam: IParam
    {
        public string Name { get; private set; }
        public string Title { get; private set; }
        public bool DefaultSelected = false;
        public string Default { get; private set; }

        public StringParam(string name, string title, string defaultValue="")
        {
            Name = name;
            Title = title;
            Default = defaultValue;
        }

        public object GetValue()
        {
            var defaultValue = String.IsNullOrEmpty(Default) ? "" : " [" + Default + "]";
            ConsoleWriter.WriteLine(String.Format("Enter value for >{0}{1}< or Enter 'q' to exit", Title, defaultValue));
            var value = Console.ReadLine();
            if (value == "q")
            {
                throw new EscapeException("Exit was triggered!");
            }
            if (String.IsNullOrWhiteSpace(value))
            {
                value = Default;
                DefaultSelected = true;
            }

            return value;
        }
    }
}
