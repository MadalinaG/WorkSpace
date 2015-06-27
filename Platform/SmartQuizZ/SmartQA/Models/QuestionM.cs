
using QuestionAnalisys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SmartQA.Models
{
    [Serializable()]
    [XmlRoot("Question")]
    public class QuestionM
    {
        [XmlElement(ElementName = "QuestionId")]
        public int QuestionId { get; set; }

        [XmlElement(ElementName = "QuizId")]
        public int? QuizId { get; set; }

        [XmlElement(ElementName = "TopicId")]
        public int? TopicId { get; set; }

        [XmlElement(ElementName = "QuestionText")]
        public string QuestionText { get; set; }

        [XmlElement(ElementName = "QuestionLemmatized")]
        public string QuestionLemmatized { get; set; }

        [XmlElement(ElementName = "QuestionAfterProcess")]
        public string QuestionAfterProcess { get; set; }

        [XmlElement(ElementName = "LanguageId")]
        public Languages LanguageId { get; set; }

        [XmlElement(ElementName = "QuestionType")]
        public QuestionType QuestionType { get; set; }

        [XmlElement(ElementName = "AnswerTypeExpected")]
        public AnswerTypeExpected? AnswerTypeExpected { get; set; }

        [XmlElement(ElementName = "SentanceW")]
        public Sentance SentanceW { get; set; }

        [XmlElement(ElementName = "AnswerList")]
        public List<Answer> AnswerList { get; set; }

        [XmlElement(ElementName = "CorrectAnswers")]
        public List<Answer> CorrectAnswers { get; set; }

        [XmlElement(ElementName = "KeyWords")]
        public List<Word> KeyWords { get; set; }

        [XmlElement(ElementName = "IsNegative")]
        public bool? IsNegative { get; set; }

        [XmlElement(ElementName = "Focus")]
        public string Focus { get; set; }

        [XmlElement(ElementName = "IsAnswered")]
        public bool? IsAnswered { get; set; }

        [XmlIgnore]
        public string KeyWord { get; set; }

        public bool ShouldSerializeQuestionType()
        {
            return QuestionType != 0;
        }
        public bool ShouldSerializeSentanceW()
        {
            return SentanceW != null;
        }
        public bool ShouldSerializeAnswerTypeExpected()
        {
            return AnswerTypeExpected != null;
        }
        public bool ShouldSerializeFocus()
        {
            return Focus != null;
        }
        public bool ShouldSerializeLanguageId()
        {
            return LanguageId != 0;
        }
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
