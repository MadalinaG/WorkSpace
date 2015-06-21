using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionAnalisys.DTO
{
    [Serializable()]
    public class S
    {      

        [XmlElement("W", IsNullable = true)]
        public W[] W { get; set; }

        [XmlElement("NP", IsNullable = true)]
        public NP[] NP { get; set; }
    }
}
