using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.io;

namespace TextProcessing
{
    public class TextDocument
    {
        public string Path { get; set; }
        public string DocumentName { get; set; }
        public string Text { get; set; }
    }
}
