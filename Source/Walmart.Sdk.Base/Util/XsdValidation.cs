using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Xml.Serialization;

namespace Walmart.Sdk.Base.Util
{
    public class XsdValidation
    {
        public static List<ValidationEventArgs> validateXml(string xsdFilePath, string xmlFilePath)
        {
            // TODO: figure out how to validate xml files with xsd schema
            XmlSchema xsd;
            using (FileStream stream = new FileStream(xsdFilePath, FileMode.Open, FileAccess.Read))
            {
                //xsd = XmlSchema.Read(stream, null);
            }
            

            return new List<ValidationEventArgs>();
        }
    }
}
