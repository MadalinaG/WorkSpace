using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestionAnalisys;
using System.Text.RegularExpressions;

namespace AnswerEvaluation
{
    public class AnswerProcess
    {
        private List<Answer> AnswerList;
        private Languages Language;

        public AnswerProcess(List<Answer> answersList, Languages language)
        {
            AnswerList = new List<Answer>();
            AnswerList = answersList;
            Language = language;

        }

        public List<Answer> ProcessAnswers()
        {
            int IndexAnswer = 0;
            Regex replacer;

            if (Language == Languages.English)
            {
                replacer = new Regex(StopWords.SWE);
            }
            else
            {
                replacer = new Regex(StopWords.SWR);
            }

            foreach (Answer answer in AnswerList)
            {
                answer.ID = ++IndexAnswer;
                answer.TextLematized = replacer.Replace(answer.Text, "");
                Regex replacer2 = new Regex(@"\b(?:[a-zA-Z]\.\s|[a-zA-Z]\)\s)\b");
                answer.TextLematized = replacer2.Replace(answer.TextLematized, "");
                answer.TextLematized = Regex.Replace(answer.TextLematized, @"[^\w\s^(ăîâșțĂÎÂȘȚ)]", "");
                answer.TextLematized = answer.TextLematized.Trim();
                answer.CleareText = replacer2.Replace(answer.Text, "");
                answer.CleareText = Regex.Replace(answer.CleareText, @"[^\w\s^(ăîâșțĂÎÂȘȚ)]", "");
            }

            return AnswerList;
        }

    }
}
