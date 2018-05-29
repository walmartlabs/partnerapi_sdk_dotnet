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
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Walmart.Sdk.Marketplace;

namespace Walmart.Sdk.Marketplace.E2ETests
{
    public abstract class BaseE2ETest
    {
        protected ClientConfig config;
        protected ApiClient client;
        protected Dictionary<string, string> settings;

        public BaseE2ETest()
        {
            LoadRawSettings();
            InitApiClient();
        }

        protected Stream LoadRequestStub(string name)
        {
            return LoadStreamFromAssembly(name + ".xml");
        }

        protected Stream LoadStreamFromAssembly(string name)
        {
            var assembly = typeof(BaseE2ETest).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
            return assembly.GetManifestResourceStream(assemblyName + "." + name );
        }

        protected void LoadRawSettings()
        {
            var json = new Newtonsoft.Json.JsonSerializer();
            using (var stream = LoadStreamFromAssembly("settings.json"))
            {
                var content = new StreamReader(stream).ReadToEnd();
                var reader = new JsonTextReader(new StringReader(content));
                settings = json.Deserialize<Dictionary<string, string>>(reader);
            }

            using (var credsStream = File.OpenText("credentials.json"))
            {
                Dictionary<string, string> creds = json.Deserialize<Dictionary<string, string>>(new JsonTextReader(credsStream));
                if (!creds.ContainsKey("ConsumerId") || !creds.ContainsKey("PrivateKey"))
                {
                    
                    throw new System.Exception("Credentials file has incorrect format!");
                }
                settings.Add("consumerId", creds["ConsumerId"]);
                settings.Add("privateKey", creds["PrivateKey"]);
            }
        }

        protected void InitApiClient()
        {
            if (settings is null)
            {
                throw new Exception("Settings weren't loaded!");
            }

            config = new Marketplace.ClientConfig(
                settings["consumerId"],
                settings["privateKey"]
            );
            config.BaseUrl = settings["BaseUrl"];
            config.ChannelType = settings["ChannelType"];
            config.ServiceName = settings["ServiceName"];
            client = new ApiClient(config);
        }
    }
}
