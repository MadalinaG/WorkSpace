using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQuizZN.Models
{
    public class Test
    {
        public int ID { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "You must upload the quiz file.", MinimumLength = 6)]
        [DataType(DataType.Upload)]
        [Display(Name = "FIleName")]
        public string FileName { get; set; }

        [Required]

        public int TopicID { get; set; }

        [Required]
        public string AddedByID { get; set; }

        [Required]
        public DateTime AddedTime { get; set; }

        [Required]
        [Range(0, 500, ErrorMessage = "Please enter valid integer Number")]
        [Display(Name = "Questions number")]
        public int QuestionsNumber { get; set; }

        [Required]
        [Display(Name = "Answers number for question")]
        public int NumberOfAnswerForQuestion { get; set; }

        [Required]
        [Display(Name = "Multiple answers for one question")]
        public bool MultipleAnswersForOneQuestion { get; set; }

        [Display(Name ="Description")]
        public string Description { get; set; }

        [Display(Name ="Quiz Instructions")]
        public string QuizInstructions { get; set; }

        public string QuizPathOnServer { get; set; }

        [Required]
        [Display(Name = "Quiz start at page:")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Please enter a valid page number.")]
        public int StartReadAtPage { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Please enter a valid page number.")]
        [Display(Name = "Quiz ends at page:")]
        public int StopReadAtPage { get; set; }

        public string XmlBeforeProcess { get; set; }

        public string XmlAfterProcess { get; set; }

        public string Query { get; set; }
    }
}