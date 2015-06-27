using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SmartQA.Models
{
    [Serializable()]
    [XmlRoot("BackgroundDocument")]
    public class BGDocument
    {
        [XmlElement(ElementName = "ID")]
        public int ID { get; set; }

        [XmlElement(ElementName = "TopicID")]
        public int TopicID { get; set; }

        [XmlElement(ElementName = "TestID")]
        public int TestID { get; set; }

        [XmlElement(ElementName = "Title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }

        [XmlElement(ElementName = "AddedTime")]
        public DateTime AddedTime { get; set; }

        [XmlElement(ElementName = "Path")]
        public string Path { get; set; }

        [XmlElement(ElementName = "AddedByID")]
        public string AddedByID { get; set; }

        [XmlElement(ElementName = "Paragraphs")]
        public string[] Paragraphs { get; set; }

        [XmlElement(ElementName = "Score")]
        public float Score { get; set; }

        [XmlElement(ElementName = "Paragraph")]
        public string Paragraph { get; set; }

        
        public string[] ProcessedParagraphs { get; set; }

        [XmlElement(ElementName = "ProcessedParagraph")]
        public string ProcessedParagraph { get; set; }
    }
}