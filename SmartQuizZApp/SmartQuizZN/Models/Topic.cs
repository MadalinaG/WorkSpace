using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQuizZN.Models
{
        public class Topic
        {
            public int ID { get; set; }

            [Required]
            [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [DataType(DataType.Text)]
            [Display(Name = "Topic Name")]
            public string TopicName { get; set; }

            [Required]
            [StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [DataType(DataType.Text)]
            [Display(Name = "Description")]
            public string Description { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedTime { get; set; }
            public int NrOfQuestions { get; set; }
            public int NrOfArticles { get; set; }
            public string PhotoName { get; set; }
            public int QuizNumber { get; set; }
        }
}