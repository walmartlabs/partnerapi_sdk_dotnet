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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Walmart.Sdk.Base.Http.Fetcher
{
    // the purpose of this class is to serve http responses
    // from local files, sdk is using this client when isSimulation modeis ON
    public class LocalFetcher: BaseFetcher
    {
        const string mappingFilename = "_mapping.json";
        const string assemblyPath = "resources.payloadMock";

        private static readonly Primitive.LoggerContainer log = Primitive.LoggerContainer.GetInstance();
        private Dictionary<string, string> mapping;
        private string customFolder = "";

        public LocalFetcher(Primitive.Config.IHttpConfig config) : base(config)
        {
            var content = LoadAssemblyFile(mappingFilename);
            mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
        }

        public void SetCustomMockFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                var exceptionMessage = string.Format("Custom Mock folder {0} for Local Fetcher is not found!", folderPath);
                log.Error(exceptionMessage);
                throw new Base.Exception.InitException(exceptionMessage);
            }

            Dictionary<string, string> newMapping;
            try
            {
                var content = File.ReadAllText(folderPath + mappingFilename);
                newMapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            }
            catch (System.Exception ex)
            {
                var exceptionMessage = string.Format("Custom folder >{0}< have invalid mapping config!", customFolder);
                log.Error(exceptionMessage);
                throw new Base.Exception.InitException(exceptionMessage, ex);
            }
            mapping = newMapping;
            customFolder = folderPath;
        }

        private string LoadFile(string filename)
        {
            if (customFolder == "")
                return LoadAssemblyFile(filename);
            else
                return File.ReadAllText(customFolder + filename);
        }

        private string LoadAssemblyFile(string filename)
        {
            var assembly = config.GetType().GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
            using (var stream = assembly.GetManifestResourceStream(assemblyName + "." + assemblyPath + "." + filename))
            {
                try
                {
                    return new StreamReader(stream).ReadToEnd();
                }
                catch (System.Exception ex)
                {
                    var exceptionMessage = string.Format("Unable to load file {0} from assembly.", filename);
                    log.Error(exceptionMessage + " Reason: " + ex.Message);
                    // pretend like we got 500
                    throw new Exception.GatewayException(exceptionMessage, ex);
                }
            }
        }

        private Response BuildResponseObject(string content, System.Net.HttpStatusCode code)
        {
            var httpResponse = new HttpResponseMessage()
            {
                Content = new StringContent(content),
                StatusCode = code
            };
            
            return new Response(httpResponse);
        }

        override public Task<IResponse> ExecuteAsync(IRequest request)
        {
            var fullKey = request.Method.Method + "_" + request.EndpointUri + request.BuildQueryParams();
            var longestMappingKey = "";
            foreach (var key in mapping.Keys)
            {
                if (fullKey.StartsWith(key) && key.Length > longestMappingKey.Length)
                {
                    longestMappingKey = key;
                }
            }
            string filename;
            if (mapping.ContainsKey(longestMappingKey))
                filename = mapping[longestMappingKey];
            else
                throw new Exception.GatewayException("Unable to find payload for a request!");

            var fileExtension = config.ApiFormat.ToString().ToLower();
            var content = LoadFile(filename + "." + fileExtension);

            var errorRegex = new Regex(@"[245][0-9]{2,2}_error$");
            var code = 200;
            if (errorRegex.IsMatch(filename))
            {
                Int32.TryParse(filename.Substring(filename.Length-9, 3), out code);
            }

            var response = BuildResponseObject(content, (System.Net.HttpStatusCode)code);
            return Task.FromResult<IResponse>(response);
        }
    }
}
