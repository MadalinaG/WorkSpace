﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQA.Models
{
    public class BGDocument
    {
        public int ID { get; set; }
        public int TopicID { get; set; }
        public int TestID { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string AddedByID { get; set; }
    }
}