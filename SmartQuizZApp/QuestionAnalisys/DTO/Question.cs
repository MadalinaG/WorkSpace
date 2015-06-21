
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QuestionAnalisys
{
    [Serializable()]
    public class Question
    {
        [XmlElement(ElementName = "QuestionId")]
        public int QuestionId { get; set; }

        [XmlElement(ElementName = "QuizId")]
        public int? QuizId { get; set; }

        [XmlElement(ElementName = "TopicId")]
        public int? TopicId { get; set; }

        [XmlElement(ElementName = "QuestionText", IsNullable = true)]
        public string QuestionText { get; set; }

        [XmlElement(ElementName = "QuestionLemmatized", IsNullable = true)]
        public string QuestionLemmatized { get; set; }

        [XmlElement(ElementName = "QuestionAfterProcess", IsNullable = true)]
        public string QuestionAfterProcess { get; set; }

        [XmlElement(ElementName = "LanguageId", IsNullable = true)]
        public Languages LanguageId { get; set; }

        [XmlElement(ElementName = "QuestionType", IsNullable = true)]
        public QuestionType QuestionType { get; set; }

        [XmlElement(ElementName = "AnswerTypeExpected", IsNullable = true)]
        public AnswerTypeExpected? AnswerTypeExpected { get; set; }

        [XmlElement(ElementName = "SentanceW", IsNullable = true)]
        public Sentance SentanceW { get; set; }

        [XmlElement(ElementName = "AnswerList", IsNullable = true)]
        public List<Answer> AnswerList { get; set; }

        [XmlElement(ElementName = "CorrectAnswers", IsNullable = true)]
        public List<Answer> CorrectAnswers { get; set; }

        [XmlElement(ElementName = "KeyWords", IsNullable = true)]
        public List<Word> KeyWords { get; set; }

        [XmlElement(ElementName = "IsNegative")]
        public bool? IsNegative { get; set; }

        [XmlElement(ElementName = "Focus", IsNullable = true)]
        public string Focus { get; set; }

        [XmlElement(ElementName = "IsAnswered")]
        public bool? IsAnswered { get; set; }

        public bool ShouldSerializeQuizId()
        {
            return QuizId != null;
        }
        public bool ShouldSerializeTopicId()
        {
            return TopicId != null;
        }
        public bool ShouldSerializeIsAnswered()
        {
            return IsAnswered != null;
        }
        public bool ShouldSerializeIsNegative()
        {
            return IsNegative != null;
        }
      
    }
}
