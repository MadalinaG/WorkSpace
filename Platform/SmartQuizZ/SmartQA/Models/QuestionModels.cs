using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQA.Models
{
    public class QuestionModels
    {
        public int ID { get; set; }
        public int QuizID { get; set; }
        public int TopicID { get; set; }
        public int DocumentID { get; set; }
        public bool MultipleAnswers { get; set; }
        public bool QuestionSolved { get; set; }
        public int NumberOfAnsers { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Question cannot be blank.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Question")]
        public string Text { get; set; }

        public List<AnswerModels> Answers { get; set; }
    }
}