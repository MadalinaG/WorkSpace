using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQA.Models
{
    public class TestModels
    {
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "You must upload the quiz file.", MinimumLength = 6)]
        [DataType(DataType.Upload)]
        [Display(Name = "FileName")]
        public string FileName { get; set; }

        [Required]
        public int TopicID { get; set; }

        [Required]
        public string AddedByID { get; set; }

        [Required]
        public DateTime AddedTime { get; set; }

        [Required]
        [Range(0, 500, ErrorMessage = "Please enter valid  number between (0, 500).")]
        [Display(Name = "Questions nr.")]
        public int QuestionsNumber { get; set; }

        [Required]
        [Display(Name = "Answers number")]
        public int NumberOfAnswerForQuestion { get; set; }

        [Required]
        [Display(Name = "Multiple answers")]
        public bool MultipleAnswersForOneQuestion { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Quiz Instructions")]
        public string QuizInstructions { get; set; }

        public string QuizPathOnServer { get; set; }

        [Required]
        [Display(Name = "Start page")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Please enter a valid page number.")]
        public int StartReadAtPage { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Please enter a valid page number.")]
        [Display(Name = "End page")]
        public int StopReadAtPage { get; set; }

        public string XmlBeforeProcess { get; set; }

        public string XmlAfterProcess { get; set; }

        public string Query { get; set; }
        public string TopicName { get; set; }
        public bool Solved { get; set; }
        public string PathTopicPicture { get; set; }
        public string UserName { get; set; }
        public List<QuestionModels> Questions { get; set; }
        public bool QuizSaved { get; set; }

    }
}