using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace QuestionAnalisys
{
    [Serializable()]
    public class KeyWord
    {
        [XmlElement(ElementName = "Relevance")]
        public double Relevance { get; set; }

        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }
    }
}
