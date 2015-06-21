using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIs;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using QuestionAnalisys.DTO;
namespace QuestionAnalisys
{
    public class Analyser
    {
        public List<Question> QuestionList;
        private string Query;
        private Services service;

        public Analyser(List<Question> questionList, string query)
        {
            QuestionList = questionList;
            Query = query;
        }

        public void AnalizeQuestions()
        {
            string lang = string.Empty;
            string parid = string.Empty;
            string langu = string.Empty;
            Regex replacer;
            Languages language = GetLanguageForQuestion(Query);

            if (language == Languages.English)
            {
                replacer = new Regex(StopWords.SWE);
                langu = "en";
            }
            else
            {
                replacer = new Regex(StopWords.SWR);
                langu = "ro";
            }

            Query = replacer.Replace(Query, "");

            string[] QuestionsAfterProcess = Query.Split('.');

            for (int i = 0; i < QuestionList.Count; i++)
            {
                Question question = QuestionList[i];
                question.QuestionAfterProcess = QuestionsAfterProcess[i];
                question.LanguageId = language;

                if (question.LanguageId == Languages.Romanian)
                {
                    lang = "ro";
                    parid = "ro";
                }
                else
                {
                    lang = "en";
                    parid = "eng";
                }

                question = BuildSentence(question, lang, parid);
                question.QuestionLemmatized = GETQuestionLematized(question.SentanceW);

                if (lang == "en")
                {
                    if (question.QuestionType == 0)
                    {
                        QuestionProcessEN qpEN = new QuestionProcessEN(question);
                        question.IsNegative = qpEN.IsNegativeQuestion();
                        question.QuestionType = qpEN.GetQuestionType();
                        question.AnswerTypeExpected = qpEN.GetAnswerType();
                    }
                    else
                    {
                        question.AnswerTypeExpected = (question.QuestionType == QuestionType.FactoidPerson) ? AnswerTypeExpected.Person : AnswerTypeExpected.Location;
                    }
                }
                else if (lang == "ro")
                {
                    if (question.QuestionType == 0)
                    {
                        QuestionProcessRO qpRO = new QuestionProcessRO(question);
                        question.IsNegative = qpRO.IsNegativeQuestion();
                        question.QuestionType = qpRO.GetQuestionType();
                        question.AnswerTypeExpected = qpRO.GetAnswerType();
                    }
                    else
                    {
                        question.AnswerTypeExpected = (question.QuestionType == QuestionType.FactoidPerson) ? AnswerTypeExpected.Person : AnswerTypeExpected.Location;
                    }
                }
            }
        }
         
        private Question BuildSentence(Question question, string lang, string parid)
        {
            string response = string.Empty;
            Languages language;
            if(lang == "ro")
            {
                response = GetLemmaRO(question.QuestionAfterProcess, lang, parid);
                language = Languages.Romanian;
            }
            else
            {
                response = GetLemmaEN(question.QuestionAfterProcess, lang, parid);
                language = Languages.English;
            }

            DOCUMENT SentenceQuestion = HttpHandlers.Deserialize<DOCUMENT>(response);

            ChankQuestion chanck = new ChankQuestion(response,language);
            if (service == Services.NamedEntityRecognizerWS)
            {
                question.SentanceW = chanck.GetQuestionProcessed(SentenceQuestion);
                question.KeyWords = chanck.KeyWords.OrderByDescending(o => o.Score).ToList();
                question.Focus = chanck.Focus;
                question.QuestionType = chanck.type;
            }
            else if (service == Services.Racai)
            {
                question.SentanceW = chanck.GetLemma();
                question.Focus = chanck.Focus;
            }

            return question;
        }

        private string GetLemmaRO(string question, string lang, string parid)
        {
           string response = string.Empty;
           if (!string.IsNullOrEmpty(question))
           {
                   var chunck = new NamedEntityRecognizerWS.NamedEntityRecognizerWS();
                   chunck.Timeout = 5000;
                   response = chunck.recognizeEntities(question, lang);

                       if(string.IsNullOrEmpty(response))
                       {
                           var ttl = new  Racai.TTL();
                           string sgmlQuestion = ttl.UTF8toSGML(question);

                           if (!string.IsNullOrEmpty(sgmlQuestion))
                           {
                               sgmlQuestion = ttl.XCES(lang, parid, sgmlQuestion);

                               if(!string.IsNullOrEmpty(sgmlQuestion))
                               {
                                   response = ttl.SGMLtoUTF8(sgmlQuestion);
                                   if(!string.IsNullOrEmpty(response))
                                   {
                                       service = Services.Racai;
                                   }
                               }
                           }
                       }
                       else
                       {
                           service = Services.NamedEntityRecognizerWS;
                       }
               
           }
           return response;
        }

        private string  GetLemmaEN(string question,string lang, string parid)
        {
           string response = string.Empty;

           if(!string.IsNullOrEmpty(question))
           {
                   var lema = new NamedEntityRecognizerWS.NamedEntityRecognizerWS();
                   lema.Timeout = 5000;
                   response = lema.recognizeEntities(question, lang);

                   if (string.IsNullOrEmpty(response))
                   {
                       var ttl = new Racai.TTL();
                       string sgmlQuestion = ttl.UTF8toSGML(question);

                       if (!string.IsNullOrEmpty(sgmlQuestion))
                       {
                           sgmlQuestion = ttl.XCES(lang, parid, sgmlQuestion);
                           if (!string.IsNullOrEmpty(sgmlQuestion))
                           {
                               response = ttl.SGMLtoUTF8(sgmlQuestion);

                               if (!string.IsNullOrEmpty(response))
                               {
                                   service = Services.Racai;
                               }
                           }
                       }
                   }
                   else
                   {
                       service = Services.NamedEntityRecognizerWS;
                   }
               
           }

           return response;
        }

        private List<KeyWord> GETKeyWords(string questionText, Languages language)
        {
            List<KeyWord> keywordsList = new List<KeyWord>();
            Keywords keywords = new Keywords(questionText.Remove(0, questionText.IndexOf('.') + 1));
            string xml = keywords.GetKeywords();
            ChankQuestion chanck = new ChankQuestion(xml,language);
            keywordsList = chanck.GETKeyWordsQ();
            return keywordsList;
        }

        private Languages GetLanguageForQuestion(string questionText)
        {
            Languages language = Languages.Romanian;
            if (!string.IsNullOrEmpty(questionText))
            {
                Language getLanguage = new Language(questionText);
                string languageXml = getLanguage.GetLanguage();

                if (!string.IsNullOrEmpty(languageXml))
                {
                    XDocument xmlDocument = XDocument.Parse(languageXml);
                    XElement[] elements = xmlDocument.Element("results").Elements().ToArray();
                    XElement languageElement = elements.ElementAtOrDefault(3);
                    if(languageElement.Value == "romanian")
                    {
                       return QuestionAnalisys.Languages.Romanian;
                    }
                    else if (languageElement.Value == "english")
                    {
                        return QuestionAnalisys.Languages.English;
                    }
                }
            }
            return language;
        }

        private string GETQuestionLematized(Sentance sentence)
        {
            string questionLematized = string.Empty;
            if(sentence.Words.Count > 0)
            {
                foreach(Word word in sentence.Words.OrderBy(o => o.offset).ToList())
                {
                    questionLematized += word.Value + " ";
                }
            }
            return questionLematized.Trim();
        }

    }
}
