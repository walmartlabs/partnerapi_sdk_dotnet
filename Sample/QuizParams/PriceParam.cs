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

namespace Walmart.Sdk.Marketplace.Sample.QuizParams
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException(string message): base(message)
        { }
    }

    public class PriceParam: IParam
    {
        public string Name { get; private set; }
        public string Title { get; private set; }

        public PriceParam(string name, string title)
        {
            Name = name;
            Title = title;
        }

        public object GetValue()
        {
            ConsoleWriter.WriteLine(String.Format("Enter value for {0} or type 'q' to exit", Title));
            double value = -1.0;
            do
            {
                string rawInput = "";
                try
                {
                    rawInput = Console.ReadLine();
                    if (!String.IsNullOrWhiteSpace(rawInput))
                    {
                        if (rawInput == "q")
                        {
                            throw new EscapeException("Operation was cancelled!");
                        }
                        if (rawInput.IndexOf(".") == -1)
                        {
                            rawInput += ".0";
                        }

                        value = Double.Parse(rawInput);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.WriteLine(String.Format("Unable to parse Price >{0}<", rawInput));
                    continue;
                }

                if (value <= 0.000000000001)
                {
                    ConsoleWriter.WriteLine(String.Format("Invalid value >{0}< for Item price", value));
                }
                else
                {
                    break;
                }
            } while (true);

            if (value <= 0)
            {
                throw new InvalidValueException(String.Format("Invalid value >{0}< for Item price", value));
            }

            return value;
        }
    }
}
