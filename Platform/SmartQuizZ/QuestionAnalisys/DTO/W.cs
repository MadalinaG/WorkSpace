using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionAnalisys.DTO
{
    [Serializable()]
    public class W
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute("LEMMA")]
        public string LEMMA { get; set; }

        [XmlAttribute("POS")]
        public string POS { get; set; }

        [XmlAttribute("offset")]
        public int offset { get; set; }
    }
}
