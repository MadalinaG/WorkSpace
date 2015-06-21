using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionAnalisys
{
    public class QuestionProcessEN
    {
        public readonly Question Question;
        
        public QuestionProcessEN(Question _question)
        {
            Question = _question;
        }

        public bool IsNegativeQuestion()
        {
            Regex regex = new Regex(@"(not)");
            Regex regex2 = new Regex("with one exception");
            Regex regex3 = new Regex("except");
            Regex regex4 = new Regex("incorrect");

            string q = Regex.Replace(Question.QuestionText.ToLower(), @"[^\w\s]", "");

            Match match = regex.Match(q);
            Match match2 = regex2.Match(q);
            Match match3 = regex3.Match(q);
            Match match4 = regex4.Match(q);

            if(match.Success || match2.Success || match3.Success || match4.Success)
            {
                return true;
            }
            return false;
        }

        private bool IsWhichIsTrue()
        {
            string questionText = Regex.Replace(Question.QuestionText.ToLower(), @"[^\w\s]", "");  

            Regex regex = new Regex(@"(which of the following)");
            Regex regex2 = new Regex(@"which one of the following");
            Match match = regex.Match(questionText);
            Match match2 = regex2.Match(questionText);
            if (match.Success || match2.Success)
            {
                return true;
            }
            else
            {
                regex = new Regex(@"\bwhich\b");
                match = regex.Match(questionText);
                if (match.Success)
                {
                    return true;
                }
                else
                {
                    regex = new Regex(@"\bwhat\b");
                    match = regex.Match(questionText);
                    if(match.Success && IsMethod() == false && IsReason() == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsReason()
        {
            Regex regex = new Regex(@"\bwhat\b");
            Regex regex1 = new Regex(@"\bwhy\b");

            string questionText = Regex.Replace(Question.QuestionText.ToLower(), @"[^\w\s]", "");

            Match match = regex1.Match(questionText);
            if(match.Success)
            {
                return true;
            }
            else
            {
                match = regex.Match(questionText);
                 {
                     if(match.Success)
                     {
                         Regex regex2 = new Regex("reason");
                         Regex regex3 = new Regex("purpose");

                         match = regex2.Match(questionText);
                         Match match2 = regex3.Match(questionText);

                         if(match.Success || match2.Success)
                         {
                             return true;
                         }
                     }
                 }
            }
            return false;
        }

        private bool IsMethod()
        { //daca contine how dare nu e langa verb inseamna ca e factoid

            string questionText = Regex.Replace(Question.QuestionText.ToLower(), @"[^\w\s]", "");

            Regex regex = new Regex(@"\bhow\b");
            Match match = regex.Match(questionText);
            if(match.Success) 
            {
                if(IsVerb() == true)
                {
                    return true;
                }
            }

            Regex regex2 = new Regex(@"\bwhat\b");
            match = regex2.Match(questionText);

            if(match.Success)
            {
                Regex regex3 = new Regex("method");
                Regex regex4 = new Regex("way");

                match = regex3.Match(questionText);
                Match match2 = regex4.Match(questionText);

                if (match.Success || match2.Success)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsVerb()
        {
            Sentance S = Question.SentanceW;
            for (int i = 0; i < S.Words.Count;i++ )
            {
                if (S.Words[i].LEMMA == "how")
                {
                    if(S.Words[i + 1].POS == PartOfSpeech.VERB)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsCausal()
        {
            string[] patterns = new string[] { "cause", "lead", "generate", "make", "effect" };
            string questionText = Regex.Replace(Question.QuestionText.ToLower(), @"[^\w\s]", "");

            Regex regex = new Regex(@"\bwhat\b");
            Match match = regex.Match(questionText);

            if(match.Success)
            {
                regex = new Regex("could");
                match = regex.Match(questionText);
                if(match.Success)
                {
                    return true;
                }

                foreach(Word w in  Question.SentanceW.Words)
                {
                    foreach(string p in patterns)
                    {
                        regex = new Regex(p);
                        match = regex.Match(w.LEMMA);
                        if(match.Success)
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        private QuestionType  FactoidType()
        {
            string questionText = Regex.Replace(Question.QuestionText.ToLower(), @"[^\w\s]", "");

            QuestionType type = QuestionType.Other;
            //location 
            Regex regex = new Regex(@"\bwhere\b");
            Match match = regex.Match(questionText);
            if(match.Success)
            {
                type = QuestionType.FactoidLocation;
            }

            regex = new Regex(@"\bwhich\b");
            match = regex.Match(questionText);
            if (match.Success)
            {
                string[] locations = new string[] { "place", "city", "location", "country", "village" };
                foreach(string location in locations)
                {
                    regex = new Regex(location);
                    match = regex.Match(questionText);
                    if(match.Success)
                    {
                        type = QuestionType.FactoidLocation;
                    }
                }
                
            }

            //person

            regex = new Regex(@"who");
            match = regex.Match(questionText);
            if (match.Success)
            {
                type = QuestionType.FactoidPerson;
            }

             regex = new Regex(@"which");
             match = regex.Match(questionText);
            if (match.Success)
            {
                string[] persons = new string[] { "person", "man", "woman", "customer", "human" };
                foreach (string person in persons)
                {
                    regex = new Regex(person);
                    match = regex.Match(questionText);
                    if (match.Success)
                    {
                        type = QuestionType.FactoidPerson;
                    }
                }

            }

            //time
            regex = new Regex(@"\bwhen\b");
             match = regex.Match(questionText);
            if (match.Success)
            {
                type = QuestionType.FactoidTime;
            }

            regex = new Regex(@"\bwhat\b");
             match = regex.Match(questionText);
            if (match.Success)
            {
                string[] times = new string[] { "\btime\b", "\bdate\b", "\bera\b", "\bmoment\b", "\bmonth\b", "\bage\b", "\byear\b", "\bday\b", "\bseason\b", "\bstart\b", "\bend\b" };
                foreach (string time in times)
                {
                     regex = new Regex(time);
                     match = regex.Match(questionText);
                    if (match.Success)
                    {
                        type = QuestionType.FactoidTime;
                    }
                }
            }

            //number
             regex = new Regex(@"how many");
             Regex regex2 = new Regex("how much");
             match = regex.Match(questionText);
             Match match2 = regex2.Match(questionText);
            if (match.Success || match2.Success)
            {
                type = QuestionType.FactoidNumber;
            }

            regex = new Regex(@"\bwhat\b");
             match = regex.Match(questionText);
            if (match.Success)
            {
                string[] numbers = new string[] { "\bnumber\b", "\bquantity\b", "\bbatch\b", "\bbulk\b", "\bcapacity\b", "\blength\b", "\bvolume\b", "\bportion\b", "\bsize\b", "\bquota\b", "\babundance\b" };
                foreach (string number in numbers)
                {
                     regex = new Regex(number);
                     match = regex.Match(questionText);
                    if (match.Success)
                    {
                        type = QuestionType.FactoidNumber;
                    }
                }
            }
            return type;
        }

        public QuestionType GetQuestionType()
        {
            QuestionType questionType = QuestionType.Other;

            questionType = FactoidType();

            if(questionType == QuestionType.Other)
            {
                if(IsCausal() == true)
                {
                    questionType = QuestionType.Causal;
                }
                else if(IsMethod() == true)
                {
                    questionType = QuestionType.Method;
                }
                else if(IsReason() == true)
                {
                    questionType = QuestionType.Reason;
                }
                else if(IsWhichIsTrue() == true)
                {
                    questionType = QuestionType.WhichIsTrue;
                }
            }
            if(questionType == QuestionType.Other)
            {
                questionType = QuestionType.WhichIsTrue;
            }
            return questionType;
        }

        public AnswerTypeExpected GetAnswerType()
        {
            if(Question.QuestionType == QuestionType.Method)
            {
                return AnswerTypeExpected.Method;
            }

            if (Question.QuestionType == QuestionType.Reason)
            {
                return AnswerTypeExpected.Reason;
            }

            if (Question.QuestionType == QuestionType.FactoidLocation)
            {
                return AnswerTypeExpected.Location;
            }

            if (Question.QuestionType == QuestionType.FactoidNumber)
            {
                return AnswerTypeExpected.Number;
            }

            if (Question.QuestionType == QuestionType.FactoidPerson)
            {
                return AnswerTypeExpected.Person;
            }

            if (Question.QuestionType == QuestionType.FactoidTime)
            {
                return AnswerTypeExpected.Date;
            }
            if (Question.QuestionType == QuestionType.Causal)
            {
                return AnswerTypeExpected.Cause;
            }
            return AnswerTypeExpected.Definition;
        }
    }
}
