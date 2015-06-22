using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionAnalisys.DTO
{
   [Serializable()]
    public class NP
    {
        [XmlAttribute("TYPE")]
        public string TYPE { get; set; } 

        [XmlElement("NP", IsNullable = true)]
        public NP[] NPP { get; set; }

        [XmlElement("HEAD",IsNullable = false)]
        public Head Head { get; set; }

       [XmlElement("W", IsNullable = true)]
        public W[] W { get; set; }
    }
}
