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

namespace Walmart.Sdk.Base.Serialization
{
    using System.IO;
    using System.Text;
    using Walmart.Sdk.Base.Primitive;

    public class XmlSerializer : ISerializer
    {
        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        public TPayload Deserialize<TPayload>(string content)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TPayload));
            using (var reader = new StringReader(content))
            {
                return (TPayload)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Converts to xml string and returns
        /// </summary>
        /// <returns></returns>
        public string Serialize<TPayload>(TPayload item)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TPayload));
            var stringWriter = new Utf8StringWriter();
            if (((IPayload) item).Xmlns.Count > 0)

                serializer.Serialize(stringWriter, item, ((IPayload) item).Xmlns);
            else
                serializer.Serialize(stringWriter, item);

            return stringWriter.ToString();
        }
    }
}
