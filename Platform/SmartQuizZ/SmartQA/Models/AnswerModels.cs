using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQA.Models
{
    public class AnswerModels
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public int QuizID { get; set;}

        
        
        [DataType(DataType.Text)]
        [Display(Name = "Answer")]
        public string Text { get; set; }

        public bool ISCorect { get; set; }
    }
}