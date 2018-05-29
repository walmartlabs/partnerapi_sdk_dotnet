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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Walmart.Sdk.Base.Primitive;
using Walmart.Sdk.Marketplace.Sample.Controllers;

namespace Walmart.Sdk.Marketplace.Sample
{
    public class Application
    {
        public Configuration Config;
        public MenuBuilder Menu;
        private ApiClient Client;
        private ClientConfig ClientConfig;
        private List<IController> Operations;
        private Dictionary<string, IController> OperationsMap = new Dictionary<string, IController>();
        private IController Current;
        private OperationQuiz Quiz = new OperationQuiz();
        private List<string> Breadcrumbs = new List<string>() {"MARKETPLACE"};

        public void GoInsideSubMenu(string name)
        {
            Breadcrumbs.Add(name);
        }

        public void GoToUpMenu()
        {
            Breadcrumbs.RemoveAt(Breadcrumbs.Count - 1);
        }

        private void InitSdk()
        {
            var ClientConfig = new ClientConfig(
                Config.Creds.ConsumerId,
                Config.Creds.PrivateKey
            );
            if (!String.IsNullOrWhiteSpace(Config.BaseUrl))
            {
                ClientConfig.BaseUrl = Config.BaseUrl;
            }
            if (!String.IsNullOrWhiteSpace(Config.ChannelType))
            {
                ClientConfig.ChannelType = Config.ChannelType;
            }
            if (!String.IsNullOrWhiteSpace(Config.ServiceName))
            {
                ClientConfig.ServiceName = Config.ServiceName;
            }
            

            Client = new ApiClient(ClientConfig)
            {
                SimulationEnabled = Config.Simulation,
                Logger = Config.Logging ? (ILoggerAdapter)new LoggerAdapter() : new NullLogger()
            };
        }

        private void ShowWalmartMessage()
        {
            var origColor = Console.ForegroundColor;
            var assembly = Assembly.GetExecutingAssembly();
            var item = assembly.GetManifestResourceStream(typeof(Program).Namespace + ".resources.startupMsg.txt");
            var content = new StreamReader(item).ReadToEnd();
            var colors = new List<ConsoleColor>()
            {
                ConsoleColor.Blue,
                ConsoleColor.Yellow
            };
            Console.CursorVisible = false;
            for (var i = 0; i < 5; i++)
            {
                Console.Clear();
                Console.ForegroundColor = colors[i % 2];
                ConsoleWriter.WriteLine(content);
                Thread.Sleep(200);
            }

            Console.Clear();
            Console.ForegroundColor = origColor;
        }

        private void InitMenu()
        {
            //ShowWalmartMessage();

            Operations = new List<IController>()
            {
                new Feed(Client),
                new Item(Client),
                new Price(Client),
                new Promotion(Client),
                new Order(Client),
                new Inventory(Client),
                new Settings(Client)
            };
            foreach (var operation in Operations)
            {
                var option = operation.GetMainMenuOption();
                OperationsMap.Add(option.Shortcut, operation);
            }
            Menu = new MenuBuilder(Operations);
        }

        public Application()
        {
            LogManager.GetLogger(typeof(Program));
            Config = Configuration.Load();
            InitSdk();
            InitMenu();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Console.CursorVisible = true;
            Console.WriteLine("I'm out of here");
            Thread.Sleep(5000);
        }

        public void Run()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Console.CursorVisible = false;
            Console.Clear();
            while (true)
            {
                Console.Clear();
                Menu.PrintMainMenu();
                ConsoleKeyInfo key;

                try
                {
                    key = ReadOneChar(OperationsMap.Keys.ToArray().ToList());
                }
                catch (Exception e)
                {
                    continue;
                }
                

                var command = key.KeyChar.ToString();
                if (!OperationsMap.ContainsKey(command)) continue;

                Current = OperationsMap[command];
                try
                {
                    ControllerLoop();
                }
                catch (EscapeException ex)
                {
                    GoToUpMenu();
                    Console.Clear();
                }
                
            }
            
        }

        private void ControllerLoop()
        {
            GoInsideSubMenu(Current.Header);
            Console.Clear();

            var availableKeys = new List<string>();
            foreach (var item in Current.GetControllerMenu())
            {
                availableKeys.Add(item.Shortcut);
            }

            var result = "";
            while (Current != null)
            {
                Console.Clear();
                Menu.PrintControllerMenu(Current);

                var cmd = ReadOneChar(availableKeys);
                var command = Current.GetCommandByName(cmd.KeyChar.ToString());
                Dictionary<string, object> args;
                try
                {
                    args = Quiz.Run(command.Params);
                }
                catch (Exception e)
                {
                    ConsoleWriter.WriteLine(String.Format("Unable to collect parameters >{0}<", e.Message));
                    continue;
                }

                result = command.Handler(args);
                if (!String.IsNullOrWhiteSpace(result))
                {
                    ConsoleWriter.WriteLine("You can see result below:");
                    ConsoleWriter.WriteLine("");
                    ConsoleWriter.WriteLine(result);
                    ConsoleWriter.WriteLine("");
                    ConsoleWriter.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            
        }

        public string ReadFromConsole(string promptMessage = "")
        {
            var prompt = String.Join(" >> ", Breadcrumbs);
            // Show a prompt, and get input:
            Console.Write(prompt + " " + promptMessage);
            return Console.ReadLine();
        }

        public ConsoleKeyInfo ReadOneChar(List<string> whiteList)
        {
            var prompt = String.Join(">>", Breadcrumbs);
            // Show a prompt, and get input:
            ConsoleWriter.WriteLine("");
            Console.Write(prompt + "> ");
            ConsoleKeyInfo key;
            bool needToClearError = false;
            do
            {
                key = Console.ReadKey(true);

                // means we already read the key
                if (key.KeyChar != '\0' && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
                {
                    var cmdKey = "";
                    if (key.Key == ConsoleKey.Enter)
                        cmdKey = "Enter";
                    else if (key.KeyChar == '+')
                    {
                        ShowWalmartMessage();
                        throw new EscapeException("Escape was pressed!");
                    }
                    else
                        cmdKey = key.KeyChar.ToString();
                    
                    var currPos = Console.CursorLeft;
                    Console.Write(string.Format(">{1}< {2,-30}","This ", cmdKey, "command is not supported!"));
                    Console.CursorLeft = currPos;
                    needToClearError = true;
                }
                
                
                if (key.Key == ConsoleKey.Escape)
                {
                    throw new EscapeException("Escape was pressed!");
                }
            }
            while (!whiteList.Contains(key.KeyChar.ToString()));

            if (needToClearError)
                Console.Write(string.Format("{0,-30}", ""));

            ConsoleWriter.WriteLine("");
            return key;
        }
    }
}
