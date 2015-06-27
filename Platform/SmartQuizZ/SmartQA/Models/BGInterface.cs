using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SmartQA.Models
{
    [Serializable()]
    [XmlRoot(ElementName = "BackgroundsDocument")]
    public class BGInterface
    {
        [XmlArrayItem(ElementName = "BackgroundDocument")]
        public List<BGDocument> BackgroundDocument { get; set; }
    }
}