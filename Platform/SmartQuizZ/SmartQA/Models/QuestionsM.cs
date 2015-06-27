using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SmartQA.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Questions")]
    public class QuestionsM
    {
        [XmlArrayItem(ElementName = "Question")]
        public List<QuestionM> QuestionsList { get; set; }
    }
}