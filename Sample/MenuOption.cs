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
using System.Dynamic;
using System.Linq;
using System.Text;
using Walmart.Sdk.Marketplace.V2.Payload.Price;

namespace Walmart.Sdk.Marketplace.Sample
{
    public class MenuOption : IMenuOption
    {
        public string Shortcut { get; }

        private string name;
        public string Name => name + (PostfixHandler != null ? PostfixHandler.Invoke() : "");
        public Func<string> PostfixHandler;

        public MenuOption(string key, string name, Func<System.String> postfixFunc = null)
        {
            Shortcut = key;
            this.name = name;
            PostfixHandler = postfixFunc;
        }

        public string RenderOption(int boxLength) =>
            string.Format("{0}. {1,-" + (boxLength-3).ToString() + "}", Shortcut, Name);
    }
}