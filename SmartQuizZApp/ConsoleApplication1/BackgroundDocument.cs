using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexerLucene
{
    public class BackgroundDocument
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int TopicID { get; set; }
        public int TestID { get; set; }
        public int AddedByID { get; set; }
        public DateTime AddedTime { get; set; }
        public string FileName { get; set; }
        public string[] Paragraphs { get; set; }
        public float Score { get; set; }
        public string Paragraph { get; set; }
        public string Path { get; set; }
        public string[] ProcessedParagraphs { get; set; }
        public string ProcessedParagraph { get; set; }
    }
}
