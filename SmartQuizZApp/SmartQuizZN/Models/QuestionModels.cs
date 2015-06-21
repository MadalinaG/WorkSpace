using QuestionAnalisys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQuizZN.Models
{
    public class QuestionModels
    {
        public int QuestionId { get; set; }
        public int? QuizId { get; set; }
        public int? TopicId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionLemmatized { get; set; }
        public string QuestionAfterProcess { get; set; }
        public Languages LanguageId { get; set; }
        public QuestionType QuestionType { get; set; }
        public AnswerTypeExpected? AnswerTypeExpected { get; set; }
        public Sentance SentanceW { get; set; }
        public List<Answer> AnswerList { get; set; }
        public List<Answer> CorrectAnswers { get; set; }
        public List<Word> KeyWords { get; set; }
        public bool? IsNegative { get; set; }
        public string Focus { get; set; }
        public bool? IsAnswered { get; set; }
    }
}