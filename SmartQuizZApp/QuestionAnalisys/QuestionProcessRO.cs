using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionAnalisys
{
    public class QuestionProcessRO
    {
        private Question Question;

        public QuestionProcessRO(Question _question)
        {
            Question = _question;
        }

        public bool IsNegativeQuestion()
        {
            Regex regex = new Regex(@"(\snu\s)");
            Regex regex2 = new Regex("excep");
            Regex regex3 = new Regex(@"(\sexcepție\s)");
            Regex regex4 = new Regex(@"(\sexceptie\s)");
            Match match = regex.Match(Question.QuestionText.ToLower());
            Match match2 = regex2.Match(Question.QuestionText.ToLower());
            Match match3 = regex3.Match(Question.QuestionText.ToLower());
            Match match4 = regex4.Match(Question.QuestionText.ToLower());
            if (match.Success || match2.Success || match3.Success || match4.Success)
            {
                return true;
            }
            return false;
        }

        private bool IsWhichIsTrue()
        {
            Regex regex = new Regex(@"(care din următoarele)");
            Match match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                return true;
            }
            
            return false;
        }

        private bool IsReason()
        {
            Regex regex1 = new Regex(@"(de ce\s)");
            Match match = regex1.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                return true;
            }
            else
            {
                        Regex regex2 = new Regex("(motiv)");
                        Regex regex3 = new Regex("(cauza)");
                        match = regex2.Match(Question.QuestionText.ToLower());
                        Match match2 = regex3.Match(Question.QuestionText.ToLower());
                        if (match.Success || match2.Success)
                        {
                            return true;
                        }
                    
            }
            return false;
        }

        private bool IsMethod()
        { //daca contine how dare nu e langa verb inseamna ca e factoid
            Regex regex = new Regex(@"(cum\s)");
            Match match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                if (IsVerb() == true)
                {
                    return true;
                }
            }
            Regex regex2 = new Regex(@"(care)");
            match = regex2.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                Regex regex3 = new Regex("(metoda)");
                Regex regex4 = new Regex("(mod)");
                Regex regex5 = new Regex("(modalitate)");
                Regex regex6 = new Regex("(cale)");
                 match = regex3.Match(Question.QuestionText.ToLower());
                 Match match2 = regex4.Match(Question.QuestionText.ToLower());
                 Match match3 = regex5.Match(Question.QuestionText.ToLower());
                 Match match4 = regex6.Match(Question.QuestionText.ToLower());

                 if (match.Success || match2.Success || match3.Success || match4.Success)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsVerb()
        {
            Sentance S = Question.SentanceW;
            for (int i = 0; i < S.Words.Count; i++)
            {
                if (S.Words[i].LEMMA == "cum")
                {
                    if (S.Words[i + 1].POS == PartOfSpeech.VERB)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsCausal()
        {
            string[] patterns = new string[] { "(consecința)", "(urmare)", "(rezultat)", "(efect)"};
            Regex regex = new Regex(@"(care)");
            Match match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                foreach (Word w in Question.SentanceW.Words)
                {
                    foreach (string p in patterns)
                    {
                        regex = new Regex(p);
                        match = regex.Match(w.LEMMA);
                        if (match.Success)
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        private QuestionType FactoidType()
        {
            QuestionType type = QuestionType.Other;
            //location 
            Regex regex = new Regex(@"(unde)");
            Match match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                type = QuestionType.FactoidLocation;
            }

            regex = new Regex(@"(care)");
            match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                string[] locations = new string[] { "(loc)", "(oraș)", "(localitate)", "(țara)", "(sat)" };
                foreach (string location in locations)
                {
                    regex = new Regex(@location);
                    match = regex.Match(Question.QuestionText.ToLower());
                    if (match.Success)
                    {
                        type = QuestionType.FactoidLocation;
                    }
                }

            }

            //person

            regex = new Regex(@"(cine)");
            Regex regex2 = new Regex(@"(cui\s)");
            match = regex.Match(Question.QuestionText.ToLower());
            Match match2  = regex2.Match(Question.QuestionText.ToLower());
            if (match.Success || match2.Success)
            {
                type = QuestionType.FactoidPerson;
            }

            regex = new Regex(@"(care)");
            match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                string[] persons = new string[] { "(persoana)", "(persoană)", "(femeie)", "(client)", "(om)" ,"(barbat)", "(copil)"};
                foreach (string person in persons)
                {
                    regex = new Regex(person);
                    match = regex.Match(Question.QuestionText.ToLower());
                    if (match.Success)
                    {
                        type = QuestionType.FactoidPerson;
                    }
                }

            }

            //time
            regex = new Regex(@"(când)");
            match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                type = QuestionType.FactoidTime;
            }

            regex = new Regex(@"(care)");
            match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                string[] times = new string[] { "(timp)", "(data)", "(era)", "(moment)", "(luna)", "(varsta)", "(an)", "(zi)", "(sezon)", "(start)", "(sfârșit)" };
                foreach (string time in times)
                {
                    regex = new Regex(time);
                    match = regex.Match(Question.QuestionText.ToLower());
                    if (match.Success)
                    {
                        type = QuestionType.FactoidTime;
                    }
                }
            }

            //number
            regex = new Regex(@"(cât)");
            match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                type = QuestionType.FactoidNumber;
            }

            regex = new Regex(@"(ce)");
            match = regex.Match(Question.QuestionText.ToLower());
            if (match.Success)
            {
                string[] numbers = new string[] { "(număr)", "(cantitate)", "(suma)", "(volum)", "(capacitate)", "(lungime)", "(mărime)" };
                foreach (Word w in Question.SentanceW.Words)
                {
                    foreach (string number in numbers)
                    {
                        regex = new Regex(number);
                        match = regex.Match(w.LEMMA);
                        if (match.Success)
                        {
                            type = QuestionType.FactoidNumber;
                        }
                    }
                }
            }
            return type;
        }
        public QuestionType GetQuestionType()
        {
            QuestionType questionType = QuestionType.Other;

            questionType = FactoidType();

            if (questionType == QuestionType.Other)
            {
                if (IsCausal() == true)
                {
                    questionType = QuestionType.Causal;
                }
                else if (IsMethod() == true)
                {
                    questionType = QuestionType.Method;
                }
                else if (IsReason() == true)
                {
                    questionType = QuestionType.Reason;
                }
                else if (IsWhichIsTrue() == true)
                {
                    questionType = QuestionType.WhichIsTrue;
                }
            }
            if (questionType == QuestionType.Other)
            {
                questionType = QuestionType.WhichIsTrue;
            }
            return questionType;
        }

        public AnswerTypeExpected GetAnswerType()
        {
            if (Question.QuestionType == QuestionType.Method)
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

            return AnswerTypeExpected.Definition;
        }
    }
}
