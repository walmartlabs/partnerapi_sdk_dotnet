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
    public class VersionParam: SelectionParam
    {
        public VersionParam(string name, string title)
        {
            Name = name;
            Title = title;
            Options = new Dictionary<string, IMenuOption>()
            {
                { "2", new MenuOption("v2", "V2 API Version")},
                { "3", new MenuOption("v3", "V3 API Version")}
            };
        }
        public override object GetValue()
        {
            return GetSelection().Shortcut;
        }
    }
}
