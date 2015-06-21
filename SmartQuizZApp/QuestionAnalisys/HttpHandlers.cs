using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QuestionAnalisys
{
    public class HttpHandlers
    {
        public static XmlDocument Serialize<TDto>(TDto dtoObject)
        {
            if (dtoObject is XmlDocument)
            {
                return dtoObject as XmlDocument;
            }

            if (!typeof(TDto).IsSerializable)
            {
                throw new ApplicationException("Serialize: Cannot serialize a non-serializable object");
            }

            var xmlSerializer = new XmlSerializer(typeof(TDto));

            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, dtoObject);
                var doc = new XmlDocument();
                doc.LoadXml(writer.GetStringBuilder().ToString());

                return doc;
            }
        }

        
        public static string SerializeToString<TDto>(TDto dtoObject)
        {
            if (!typeof(TDto).IsSerializable)
            {
                throw new ApplicationException("Serialize: Cannot serialize a non-serializable object");
            }

            var xmlSerializer = new XmlSerializer(typeof(TDto));

            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, dtoObject);

                return writer.GetStringBuilder().ToString();
            }
        }

        public static TDto Deserialize<TDto>(XmlDocument doc)
        {
            if (!typeof(TDto).IsSerializable)
            {
                throw new ApplicationException("Deserialize: Cannot deserialize to a non-serializable object.");
            }

            var xmlSerializer = new XmlSerializer(typeof(TDto));

            using (var reader = new StringReader(doc.OuterXml))
            {
                var dtoObject = (TDto)xmlSerializer.Deserialize(reader);

                return dtoObject;
            }
        }

        public static TDto Deserialize<TDto>(string xml)
        {
            if (!typeof(TDto).IsSerializable)
            {
                throw new ApplicationException("Deserialize: Cannot deserialize to a non-serializable object.");
            }

            var xmlSerializer = new XmlSerializer(typeof(TDto));

            using (var reader = new StringReader(xml))
            {
                var dtoObject = (TDto)xmlSerializer.Deserialize(reader);

                return dtoObject;
            }
        }


    }
}
