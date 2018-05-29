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
    public class SelectionParam: IParam
    {
        public const int LINE_LENGTH = 50;
        public virtual string Name { get; protected set; }
        public virtual string Title { get; protected set; }
        protected Dictionary<string,IMenuOption> Options;

        // this method doesn't support more options than alphabet letters
        // but that should be enough
        public virtual object GetValue()
        {
            return (object) GetSelection();
        }

        public IMenuOption GetSelection()
        {
            ConsoleKeyInfo key;
            int currPos = Console.CursorLeft;

            ConsoleWriter.WriteLine("Select one of the available options:");

            foreach (var ch in Options.Keys)
            {
                ConsoleWriter.WriteLine(String.Format("{0}. {1,-40}", ch, Options[ch].Name));
            }

            while (true)
            {
                key = Console.ReadKey(true);
                var keyStr = key.KeyChar.ToString();
                if (key.Key == ConsoleKey.Escape)
                {
                    throw new EscapeException("ESC was pressed!");
                }
                else if (!Options.ContainsKey(keyStr))
                {
                    Console.Write(">{0}< {1,-30}", keyStr, " key is not supported!");
                    Console.CursorLeft = currPos;
                }
                else
                    break;
            }
            Console.Write(new String(' ', 34));
            Console.CursorLeft = currPos;

            return Options[key.KeyChar.ToString()];
        }
    }
}
