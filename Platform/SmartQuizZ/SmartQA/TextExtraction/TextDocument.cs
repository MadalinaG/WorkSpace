using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQA.TextExtraction
{
    public class TextDocument
    {
        public string Path { get; set; }
        public string DocumentName { get; set; }
        public string Text { get; set; }
    }
}