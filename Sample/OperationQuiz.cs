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

namespace Walmart.Sdk.Marketplace.Sample
{
    public class OperationQuiz
    {
        public Dictionary<string, object> Run(List<IParam> commands)
        {
            var arguments = new Dictionary<string, object>();
            if (Object.ReferenceEquals(commands, null))
            {
                return arguments;
            }
            foreach (var cmd in commands)
            {
                var value = cmd.GetValue();
                arguments.Add(cmd.Name, value);
            }

            return arguments;
        }
    }
}
