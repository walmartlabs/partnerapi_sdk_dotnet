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
using System.Text;
using System.Threading.Tasks;
using Walmart.Sdk.Base.Primitive;
using Walmart.Sdk.Base.Serialization;
using Walmart.Sdk.Marketplace;
using Walmart.Sdk.Marketplace.Sample;

namespace Walmart.Sdk.Marketplac.Sample.Base
{
    public abstract class BaseController
    {
        protected static OperationQuiz Quiz = new OperationQuiz();

        public string Header { get; }

        protected ApiClient Client;
        protected ISerializer Serializer;

        protected MenuOption MainOption;
        protected List<MenuOption> Menu;
        public Dictionary<string, Command> Commands;

        protected BaseController(ApiClient client)
        {
            Client = client;
            Serializer = new SerializerFactory().GetSerializer(client.GetEndpointConfig().ApiFormat);
            CreateMenuDefinition();
        }

        protected abstract void CreateMenuDefinition();

        public virtual MenuOption GetMainMenuOption() => MainOption;
        public virtual List<MenuOption> GetControllerMenu() => Menu;
        public virtual Command GetCommandByName(string key)
        {
            if (!Commands.ContainsKey(key))
                return null;

            return Commands[key];
        }

        protected string GetResult<PayloadType,ExceptionType>(Task<PayloadType> task)
        {
            var spinner = new Spinner();
            try
            {
                spinner.Start();
                task.Wait();
                return Serializer.Serialize(task.Result);
            }
            catch (AggregateException ex)
            {
                var exception = ex.InnerException;
                if (exception is ExceptionType)
                {
                    return "Api exception occured: " + exception.Message;
                }
                if (exception is Walmart.Sdk.Base.Exception.InitException)
                {
                    return "Client exception occured: " + exception.Message;
                }
                return "Unknown error occured: " + exception.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                spinner.Stop();
            }
        }

        protected TPayloadType GetObjectFromFile<TPayloadType>(string path)
        {
            return (TPayloadType)new System.Xml.Serialization.XmlSerializer(typeof(TPayloadType)).Deserialize(File.OpenRead(path));
        }
    }
}
