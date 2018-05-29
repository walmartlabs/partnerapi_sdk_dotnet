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
using Walmart.Sdk.Base.Primitive;
using Walmart.Sdk.Marketplace.Sample;

namespace Walmart.Sdk.Marketplace.Sample
{
    public class Configuration
    {
        public string ServiceName;
        public string ChannelType;
        public string BaseUrl;
        public bool Simulation;
        public bool Logging;
        public Credentials Creds;

        public string StartupMessage;

        public static Configuration Load()
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;

            var ds = Path.DirectorySeparatorChar;
            var customSettingsPath = Directory.GetCurrentDirectory() + ds + "settings" + ds + "settings.json";
            string content;
            if (File.Exists(customSettingsPath))
            {
                using (var stream = File.OpenRead(customSettingsPath))
                {
                    content = new StreamReader(stream).ReadToEnd();
                }
            }
            else
            {
                using (var stream = assembly.GetManifestResourceStream(assemblyName + ".defaultSettings.json"))
                {
                    content = new StreamReader(stream).ReadToEnd();
                }
            }

            var reader = new JsonTextReader(new StringReader(content));
            var json = new JsonSerializer();
            var config = json.Deserialize<Configuration>(reader);

            config.LoadCreds();
            using (var messageStream = assembly.GetManifestResourceStream(assemblyName + ".resources.startupMsg.txt"))
            {
                config.StartupMessage = new StreamReader(messageStream).ReadToEnd();
            }

            return config;
        }

        private void LoadCreds()
        {
            var ds = Path.DirectorySeparatorChar;
            var json = new JsonSerializer();
            var credsJson = new StreamReader(File.OpenRead(Directory.GetCurrentDirectory() + string.Format("{0}settings{1}credentials.json", ds, ds))).ReadToEnd();
            var credsReader = new JsonTextReader(new StringReader(credsJson));
            var creds = json.Deserialize<Dictionary<string, string>>(credsReader);
            this.Creds = new Credentials(creds["ConsumerId"], creds["PrivateKey"]);
        }
    }
}
