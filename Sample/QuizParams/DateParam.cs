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
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace Walmart.Sdk.Marketplace.Sample.QuizParams
{
    public class DateParam :IParam
    {
        public string Name { get; private set; }
        public string Title { get; private set; }
        public bool DefaultSelected = false;
        public DateTime? Default = null;

        public DateParam(string name, string title, DateTime defaultDateTime)
        {
            Name = name;
            Title = title;
            Default = defaultDateTime;
        }

        public object GetValue()
        {
            string format = "yyyy-MM-dd";
            var defaultValue = Default.HasValue ? " [" + Default.Value.ToString(format) + "]" : "";

            Console.WriteLine("Enter value for >{0}{1}< or Enter 'q' to exit", Title, defaultValue);
            DateTime? value = null;
            do
            {
                var rawInput = Console.ReadLine();
                if (rawInput == "q")
                {
                    throw new EscapeException("Exit was triggered!");
                }

                try
                {
                    if (!String.IsNullOrWhiteSpace(rawInput))
                        value = DateTime.Parse(rawInput);
                    else
                    {
                        DefaultSelected = true;
                        value = Default;
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.WriteLine("Unable to parse date, please try again");
                }
            } while (Object.ReferenceEquals(value, null));
            return value;
        }

    }
}
