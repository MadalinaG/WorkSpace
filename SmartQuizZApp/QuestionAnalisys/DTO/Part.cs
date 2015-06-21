using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionAnalisys.DTO
{
    [Serializable()]
    public class Part
    {
        [XmlElement("S")]
        public S[] S { get; set; }
    }
}
