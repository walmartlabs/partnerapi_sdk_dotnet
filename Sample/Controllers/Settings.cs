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
using System.Linq;
using System.Text;
using Walmart.Sdk.Base.Primitive;
using Walmart.Sdk.Marketplace.V2.Payload.Item;
using Walmart.Sdk.Marketplac.Sample.Base;
using Walmart.Sdk.Marketplace.V2.Payload.Item;
using Walmart.Sdk.Marketplace.Sample.QuizParams;
using Walmart.Sdk.Marketplace.V2.Api;
using Walmart.Sdk.Marketplace.V2.Api.Exception;
using Walmart.Sdk.Base.Http.Fetcher;
using System.IO;

namespace Walmart.Sdk.Marketplace.Sample.Controllers
{
    class Settings: BaseController, IController
    {
        public static ApiVersion SelectedApiVersion = ApiVersion.V3;

        public string Prompt { get; }= "Settings Module";
        public string Header { get; } = "Settings";
        protected ItemEndpoint endpointV2;
       

        public Settings(ApiClient client) : base(client)
        {
            endpointV2 = new ItemEndpoint(client);
        }

        protected override void CreateMenuDefinition()
        {
            MainOption = new MenuOption("s", "Settings");
            Menu = new List<MenuOption>()
            {
                { new MenuOption("x", "Verify Credentials") },
                { new MenuOption("t", "Toggle Simulation", () => " " + (Client.SimulationEnabled ? "(ON)" : "(OFF)")) },
                { new MenuOption("u", "Select API Version", () => " " + (SelectedApiVersion == ApiVersion.V3 ? "V3" : "V2"))},
                { new MenuOption("z", "Enable Logging", () => " " + (Client.Logger.GetType() == typeof(NullLogger) ? "(OFF)" : "(ON)")) }
            };
            Commands = new Dictionary<string, Command>()
            {
                {
                    "x", new Command(this.VerifyCredentials)
                },
                {
                    "t", new Command(this.ToggleSimulation, new List<IParam>()
                    {
                        new BooleanParam("simulation", "Simulation Mode (ON/OFF)")
                    })
                },
                {
                    "u", new Command(this.SelectApiVersion, new List<IParam>()
                    {
                        new VersionParam("version", "API version")
                    })
                },
                {
                    "z", new Command(this.EnableLogging, new List<IParam>()
                    {
                        new BooleanParam("log", "Simulation Mode (ON/OFF)")
                    })
                }
            };
        }

        public override MenuOption GetMainMenuOption()
        {
            return new MenuOption("s", "Settings");
        }

        public string ToggleSimulation(Dictionary<string, object> args)
        {
            var flag = (bool) args["simulation"];
            Client.SimulationEnabled = flag;
            return "";
        }

        public string SelectApiVersion(Dictionary<string, object> args)
        {
            var value = (string)args["version"];
            SelectedApiVersion = (value == "v2") ? ApiVersion.V2 : ApiVersion.V3;
            return "";
        }

        public string EnableLogging(Dictionary<string, object> args)
        {
            var flag = (bool)args["log"];
            if (flag)
            {
                Client.Logger = new LoggerAdapter();
            }
            else
            {
                Client.Logger = new NullLogger();
            }

            return "";
        }

        public string VerifyCredentials(Dictionary<string, object> args)
        {
            var spinner = new Spinner();
            try
            {
                spinner.Start();
                var task = endpointV2.GetAllItems(1);
                task.Wait();
            }
            catch (Exception ex)
            {
                return String.Format("Invalid Creds. Error >{0}<", ex.Message);
            }
            finally
            {
                spinner.Stop();
            }

            return "Creds are valid!";
        }
    }
}
