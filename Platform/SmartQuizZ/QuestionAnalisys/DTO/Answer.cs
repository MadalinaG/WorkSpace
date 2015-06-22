using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionAnalisys
{
    [Serializable()]
    public class Answer
    {
        [XmlElement(ElementName = "ID")]
        public int ID { get; set; }

        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }

        [XmlElement(ElementName = "TextLematized")]
        public string TextLematized { get; set; }

        [XmlElement(ElementName = "IsCorrectAnswer")]
        public bool IsCorrectAnswer { get; set; }

        [XmlElement(ElementName = "CleareText")]
        public string CleareText { get; set; }

        public List<string> Passage = new List<string>();
        public double Score { get; set; }

        public string Paragraph { get; set; }

        public int MaxNrWFinded { get; set; }

        public int NrWNeedToBeFound { get; set; }
    }
}
