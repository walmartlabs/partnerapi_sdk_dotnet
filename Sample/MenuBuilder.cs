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

    public class MenuBuilder
    {
        public const int LINE_LENGTH = 60;
        private List<IController> MainMenu;
        private IController ServiceController;
        private readonly string MenuLine = " +" + (new String('-', LINE_LENGTH + 7)) + "+";
        private readonly string EmptyLine = " |  " + (new String(' ', LINE_LENGTH + 3)) + "  | ";
        
        public MenuBuilder(List<IController> mainMenu)
        {
            MainMenu = mainMenu;
        }

        public static string CenterString(string stringToCenter, int totalLength)
        {
            return stringToCenter.PadLeft(((totalLength - stringToCenter.Length) / 2)
                                          + stringToCenter.Length)
                .PadRight(totalLength);
        }

        public void PrintMainMenu()
        {
            ConsoleWriter.WriteLine(MenuLine);
            OutputMenuLine("");
            OutputMenuLine(CenterString("--==  Walmart Marketplace ==-- ", LINE_LENGTH));
            OutputMenuLine("");
            foreach (var item in MainMenu)
            {
                var option = item.GetMainMenuOption();
                if (option.Name.ToLower() == "settings")
                {
                    OutputMenuLine("");
                }
                OutputMenuLine(option.RenderOption(LINE_LENGTH));
            }

            OutputMenuLine("");
            ConsoleWriter.WriteLine(MenuLine);
            ConsoleWriter.WriteLine("");
        }

        private void OutputMenuLine(string line)
        {
            ConsoleWriter.WriteLine(String.Format(" |    {0,-" + LINE_LENGTH.ToString() + "}   | ", line));
        }

        public void PrintControllerMenu(IController controller)
        {
            var menu = controller.GetControllerMenu();
            ConsoleWriter.WriteLine(MenuLine);
            OutputMenuLine("");
            var header = String.Format("--==  {0}  ==-- ", controller.Header);
            OutputMenuLine(CenterString(header, LINE_LENGTH));
            OutputMenuLine("");
            foreach (var option in menu)
            {
                OutputMenuLine(option.RenderOption(LINE_LENGTH));
            }
            OutputMenuLine("");
            ConsoleWriter.WriteLine(MenuLine);
            ConsoleWriter.WriteLine("");
        }
        
    }
}
