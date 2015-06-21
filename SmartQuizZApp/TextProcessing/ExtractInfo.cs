using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using QuestionAnalisys;
namespace TextProcessing
{
    public class ExtractInfo
    {
        private int QuestionNumber;
        private string DocumentText;
        private int AnswersNr;
        private string pattern = "\n \n";
        private int QuizId;
        private int TopicId;
        private char[] delimiters = new char[] { '\r', '\n' };
        public List<Question> questionList;
        public string query;

        public ExtractInfo(int quizId, int topicId, int questionNr, string text, int answersNr)
        {
            QuestionNumber = questionNr;
            DocumentText = text;
            AnswersNr = answersNr;
            QuizId = quizId;
            TopicId = topicId;
            query = string.Empty;
        }

        public void Extract()
        {
            if (!String.IsNullOrEmpty(DocumentText))
            {
                string[] paragraphs = Regex.Split(DocumentText, pattern);

                if (paragraphs.Length > 0)
                {
                    questionList = new List<Question>();
                    foreach (string block in paragraphs)
                    {
                        Question question = new Question();
                        question.TopicId = TopicId;
                        question.QuizId = QuizId;

                        if (!String.IsNullOrWhiteSpace(block))
                        {
                            string[] sentancesN = Regex.Split(block, "\n");
                            List<string>  sentances = ClearSentances(sentancesN);
                            if (sentances.Count() > 0)
                            {
                                for (int i = 0; i < sentances.Count(); i++)
                                {
                                    int j, k;
                                    string sentTrim = sentances[i].Trim();
                                    if (!String.IsNullOrWhiteSpace(sentTrim))
                                    {
                                        if (sentTrim.Trim().IndexOf('.') < 4)
                                        {
                                            int dotPosition = sentTrim.Trim().IndexOf('.');
                                            if (dotPosition > 0)
                                            {
                                                string s = sentTrim.Trim();
                                                if (s[dotPosition - 1] == ' ')
                                                {
                                                    sentTrim = s.Remove(dotPosition - 1, 1);
                                                }
                                            }
                                        }

                                        Regex regex = new Regex(@"\d+\.\s*[A-ZÎȘȚĂÂ].+");
                                        Regex regex2 = new Regex(@"\d+\)\s*[A-ZÎȘȚĂÂ].+");
                                        Match match = regex.Match(sentTrim);
                                        Match match2 = regex2.Match(sentTrim);
                                        if (match.Success || match2.Success)
                                        {
                                            if (sentTrim.EndsWith(".") || sentTrim.EndsWith("?") || sentTrim.EndsWith(":") || IsAnswer(sentances[i + 1].Trim()))
                                            {
                                                if (sentTrim.EndsWith(":") && IsAnswer(sentances[i + 1].Trim()) == false)
                                                {
                                                    //it is part of question
                                                    question.QuestionText = sentTrim;
                                                    for(int n = i + 1; n < sentances.Count(); n++)
                                                    {
                                                        if(IsAnswer(sentances[n]))
                                                        {
                                                            i = n - 1;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            question.QuestionText +=  " " + sentances[n];
                                                        }
                                                  
                                                    }
                                                }

                                                if (i < sentances.Count - 1)
                                                {
                                                    string sentTrimNexti = sentances[i + 1].Trim();
                                                    Regex regex7 = new Regex(@"[a-zA-Z]+\)\s*");
                                                    Regex regex8 = new Regex(@"[a-zA-Z]+\.\s*");
                                                    Match match7 = regex7.Match(sentTrimNexti);
                                                    Match match8 = regex8.Match(sentTrimNexti);
                                                    if ((match7.Success || match8.Success) && IsQuestion(sentTrimNexti) == false)
                                                    {

                                                        if ((sentTrimNexti.IndexOf('.') == 1 || sentTrimNexti.IndexOf(')') == 1) && Char.IsLetter(sentTrimNexti[0]))
                                                        {
                                                            if (question.QuestionText == null)
                                                            {
                                                                question.QuestionText = sentTrim;
                                                            }
                                                            List<Answer> listAnswers = new List<Answer>();
                                                            for (j = i + 1; j < sentances.Count(); j++)
                                                            {
                                                                Answer answer = new Answer();
                                                                answer.IsCorrectAnswer = false;
                                                                string sentTrimNext = sentances[j].Trim();
                                                                Regex regex3 = new Regex(@"[a-zA-Z]+\)\s*");
                                                                Regex regex4 = new Regex(@"[a-zA-Z]+\.\s*");
                                                                Match match3 = regex3.Match(sentTrimNext);
                                                                Match match4 = regex4.Match(sentTrimNext);
                                                                if ((match3.Success || match4.Success) && (sentTrimNext.IndexOf('.') == 1 || sentTrimNext.IndexOf(')') == 1) && listAnswers.Count != AnswersNr)
                                                                {
                                                                    bool flag = false;
                                                                    answer.Text = sentTrimNext;
                                                                    if(j < sentances.Count - 1)
                                                                    {
                                                                        if(IsAnswer(sentances[j + 1].Trim()) || IsQuestion(sentances[j + 1].Trim()))
                                                                        {
                                                                            flag = true;
                                                                        }
                                                                    }
                                                                    if (sentTrimNext.EndsWith(".") || sentTrimNext.EndsWith(";") || flag == true)
                                                                    {
                                                                    }
                                                                    else
                                                                    {
                                                                        for (k = j + 1; k < sentances.Count(); k++)
                                                                        {
                                                                            string sentTrimNextAns = sentances[k].Trim();
                                                                            Regex regex5 = new Regex(@"[a-zA-Z]+\)\s*");
                                                                            Regex regex6 = new Regex(@"[a-zA-Z]+\.\s*");
                                                                            Match match5 = regex5.Match(sentTrimNextAns);
                                                                            Match match6 = regex6.Match(sentTrimNextAns);
                                                                            if (match5.Success || match6.Success && (sentTrimNextAns.IndexOf('.') == 1 || sentTrimNextAns.IndexOf(')') == 1))
                                                                            {
                                                                                //if is maching it means that is mising ; from the enf of sentence 
                                                                                listAnswers.Add(answer);
                                                                                break;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Char.IsLetter(sentTrimNextAns[0]) && Char.IsLower(sentTrimNextAns[0]))
                                                                                {
                                                                                    sentTrimNextAns = sentTrimNextAns + " ";
                                                                                    answer.Text += sentTrimNextAns;
                                                                                    if (k + 1 < sentances.Count)
                                                                                    {
                                                                                        if (sentances[k + 1].Trim().IndexOf(".") == 1 || sentances[k + 1].Trim().IndexOf(")") == 1)
                                                                                        {
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    listAnswers.Add(answer);
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                        j = k ;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    break;
                                                                }
                                                                if (listAnswers.Count == AnswersNr)
                                                                {
                                                                    break;
                                                                }
                                                                listAnswers.Add(answer);

                                                            }
                                                            i = j - 1;
                                                            question.AnswerList = listAnswers;
                                                            if (question.AnswerList.Count == AnswersNr && question.QuestionText != null)
                                                            {
                                                                questionList.Add(question);
                                                                question.QuestionId = GetQuestionId(question.QuestionText.Trim());
                                                                query += ClearSentance(question.QuestionText) + ". ";
                                                                question = new Question();
                                                                question.TopicId = TopicId;
                                                                question.QuizId = QuizId;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (sentances.ElementAtOrDefault(i + 1) != null && IsQuestion(sentances[i+1].Trim()) == false)
                                                {
                                                    string sentTrimNexti = sentances[i + 1].Trim();

                                                    if ((sentTrimNexti.EndsWith(".") || sentTrimNexti.EndsWith("?") || sentTrimNexti.EndsWith(":")) && IsNotPartOfQuestion(sentTrimNexti) == false)

                                                    {
                                                        question.QuestionText = sentTrim + " " + sentTrimNexti;
                                                        i = i ;
                                                    }
                                                    else
                                                    {   question.QuestionText = sentTrim;
                                                        for(int l = i + 1 ; l < sentances.Count ; l++)
                                                        {
                                                            string partOfSentence = sentances[l].Trim();
                                                            if(IsAnswer(partOfSentence) == false)
                                                            {
                                                                question.QuestionText += " ";
                                                                question.QuestionText += partOfSentence;
                                                            }
                                                            else
                                                            {
                                                                i = l - 1;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if(question.QuestionText != null || question.AnswerList != null)
                                            {
                                                int start;
                                                List<Answer> listAnswers = new List<Answer>();
                                                if (question.AnswerList != null)
                                                {
                                                    start = i + 1;
                                                }
                                                else
                                                {
                                                    start = i;
                                                }
                                                for (j = start; j < sentances.Count(); j++)
                                                {
                                                    Answer answer = new Answer();
                                                    answer.IsCorrectAnswer = false;
                                                    string sentTrimNext = sentances[j].Trim();
                                                    Regex regex3 = new Regex(@"[a-zA-Z]+\)\s*");
                                                    Regex regex4 = new Regex(@"[a-zA-Z]+\.\s*");
                                                    Match match3 = regex3.Match(sentTrimNext);
                                                    Match match4 = regex4.Match(sentTrimNext);
                                                    if ((match3.Success || match4.Success) && (sentTrimNext.Trim().IndexOf(".") == 1 || sentTrimNext.Trim().IndexOf(")") == 1) && listAnswers.Count != AnswersNr)
                                                    {
                                                        bool flag = false;
                                                        answer.Text = sentTrimNext;
                                                        if (j < sentances.Count - 1)
                                                        {
                                                            if (IsAnswer(sentances[j + 1].Trim()) || IsQuestion(sentances[j + 1].Trim()))
                                                            {
                                                                flag = true;
                                                            }
                                                        }
                                                        if (sentTrimNext.EndsWith(".") || sentTrimNext.EndsWith(";") || flag == true)
                                                        {
                                                        }
                                                        else
                                                        {
                                                            for (k = j + 1; k < sentances.Count(); k++)
                                                            {
                                                                string sentTrimNextAns = sentances[k].Trim();
                                                                Regex regex5 = new Regex(@"[a-zA-Z]+\)\s*");
                                                                Regex regex6 = new Regex(@"[a-zA-Z]+\.\s*");
                                                                Match match5 = regex5.Match(sentTrimNextAns);
                                                                Match match6 = regex6.Match(sentTrimNextAns);
                                                                if (match5.Success || match6.Success)
                                                                {
                                                                    break;
                                                                }else
                                                                {
                                                                    if (!string.IsNullOrEmpty(sentTrimNextAns))
                                                                    {
                                                                        if (Char.IsLetter(sentTrimNextAns[0]))
                                                                        {
                                                                            sentTrimNextAns = sentTrimNextAns + " ";
                                                                            answer.Text += sentTrimNextAns;
                                                                            if (k + 1 < sentances.Count)
                                                                            {
                                                                                if (sentances[k + 1].Trim().IndexOf(".") == 1 || sentances[k + 1].Trim().IndexOf(")") == 1)
                                                                                {
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                        else {
                                                                            listAnswers.Add(answer);
                                                                            break; 
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            j = k;
                                                            
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                    if (listAnswers.Count == AnswersNr)
                                                    {
                                                        break;
                                                    }

                                                    listAnswers.Add(answer);

                                                }
                                                i = j - 1;
                                                question.AnswerList = listAnswers;
                                                if (question.AnswerList.Count == AnswersNr && question.QuestionText != null)
                                                {
                                                    questionList.Add(question);
                                                    question.QuestionId = GetQuestionId(question.QuestionText.Trim());
                                                    query += ClearSentance(question.QuestionText) + ". ";
                                                    question = new Question();
                                                    question.TopicId = TopicId;
                                                    question.QuizId = QuizId;
                                                }
                                            }
                                        }
                                        
                                    }
                                }
                            }

                        }
                        if (question.QuestionText != null && question.AnswerList != null && question.AnswerList.Count == AnswersNr)
                        {
                            questionList.Add(question);
                            question.QuestionId = GetQuestionId(question.QuestionText.Trim());
                            query += ClearSentance(question.QuestionText) + ". ";
                        }
                    }
                }
            }
        }
        public List<string> ClearSentances(string[] sentences)
        {
            List<string> ResultList = new List<string>();
            string[] patterns = new string[] { @"\.\s[a-zA-Z]\.\s", @"\.\s[a-zA-Z]\)\s", @"\?\s[a-zA-Z]\)\s", @"\?\s[a-zA-Z]\.\s", @"\:\s[a-zA-Z]\.\s", @"\:\s[a-zA-Z]\)\s", @"\s[a-zA-Z]\)\s", @"\s[a-zA-Z]\.\s"};

            string SentenceIndex = String.Empty;
            Regex regex = new Regex(@"\d+\.\s*[A-ZÎȘȚĂÂ].+");
            Regex regex2 = new Regex(@"\d+\)\s*[A-ZÎȘȚĂÂ].+");
            
            foreach (string sentance in sentences)
            {
                bool set = false;
                if (!String.IsNullOrEmpty(sentance.Trim()) && IsMachingWithRightQuestion(sentance.Trim()) == false)
                {
                    // if are nested questions split it 
              
                    if (sentance.Trim().Length > 4)
                    {
                        string FormattedString = sentance.Trim().Remove(0, 5);
                        var matches = regex.Matches(FormattedString);
                        var matches2 = regex2.Matches(FormattedString);

                        if (matches.Count > 0)
                        {
                            set = true;
                            int IndexStart;
                            int Length = 0;
                            IndexStart = 0;
                            for (int i = 0; i < matches.Count; i++)
                            {
                                Length = matches[i].Index + 5 - IndexStart;
                                if (Length > 5)
                                {
                                    ResultList.Add(sentance.Trim().Substring(IndexStart, Length));
                                }
                                IndexStart = matches[i].Index + 5;
                            }
                            if (IndexStart < sentance.Trim().Length)
                            {
                                ResultList.Add(sentance.Trim().Substring(IndexStart, sentance.Trim().Length - IndexStart));
                            }

                        }
                        if (matches2.Count > 0)
                        {
                            set = true;
                            int IndexStart;
                            int Length = 0;
                            IndexStart = 0;
                            for (int i = 0; i < matches2.Count; i++)
                            {
                                Length = matches2[i].Index + 5 - IndexStart;
                                if (Length > 5)
                                {
                                    ResultList.Add(sentance.Trim().Substring(IndexStart, Length));
                                }
                                IndexStart = matches2[i].Index + 5;
                            }
                            if (IndexStart < sentance.Trim().Length)
                            {
                                ResultList.Add(sentance.Trim().Substring(IndexStart, sentance.Trim().Length - IndexStart));
                            }

                        }

                        foreach (string pattern in patterns)
                        {
                            Regex regexAns = new Regex(pattern);
                            var matchesAns = regexAns.Matches(sentance.Trim().Remove(0, 3));
                            if (matchesAns.Count > 0)
                            {
                                set = true;
                                int start = 0;
                                int length = 0;
                                foreach (Match match in matchesAns)
                                {
                                    length = match.Index + 4 - start;
                                    if (length > 3)
                                    {
                                        ResultList.Add(sentance.Trim().Substring(start, length));
                                    }
                                    start = match.Index + 4;
                                }
                                if (start < sentance.Trim().Length)
                                {
                                    ResultList.Add(sentance.Trim().Substring(start, sentance.Trim().Length - start));
                                }
                                break;
                            }
                        }
                        if (set == false && !String.IsNullOrEmpty(sentance.Trim()))
                        {
                            if (!string.IsNullOrEmpty(sentance.Trim()))
                            {
                                ResultList.Add(sentance.Trim());
                            }
                        }
                    }
                }
            }

            if(ResultList.Count > 0)
            {
                return ResultList;
            }
            else
            {
                foreach (string sentance in sentences)
                {
                    if (!string.IsNullOrEmpty(sentance.Trim()))
                    {
                        ResultList.Add(sentance.Trim());
                    }
                }
                return ResultList; 
            }
        }
        public bool IsNotPartOfQuestion(string sentance)
        {
            bool flag = false;
            sentance = sentance.Trim();
            string[] patterns = new string[] { @"[a-zA-Z]+\.\s*", @"[a-zA-Z]+\)\s*", @"\d+\.\s*[A-ZÎȘȚĂÂ].+", @"\d+\)\s*[A-ZÎȘȚĂÂ].+"};
            for (int i = 0; i < patterns.Length; i++)
            {
                Regex regex = new Regex(patterns[i]);
                Match match = regex.Match(sentance);
                if (match.Success)
                {
                    if (i == 0 || i == 1)
                    {
                        if ((sentance.IndexOf('.') == 1 || sentance.IndexOf(')') == 1))
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public bool IsAnswer(string sentance)
        {
            bool IsAns = false;
            string[] patterns = new string[] { @"[a-zA-Z]+\.\s*", @"[a-zA-Z]+\)\s*" };
            for (int i = 0; i < patterns.Length; i++)
            {
                Regex regex = new Regex(patterns[i]);
                Match match = regex.Match(sentance);
                if (match.Success && (sentance.IndexOf(".") == 1 || sentance.IndexOf(")") == 1))
                {
                    IsAns = true;
                }
            }
            return IsAns;
        }
        public bool IsQuestion(string sentance)
        {
            bool IsAns = false;
            string[] patterns = new string[] { @"\d+\.\s*[A-ZÎȘȚĂÂ].+", @"\d+\)\s*[A-ZÎȘȚĂÂ].+" };
            for (int i = 0; i < patterns.Length; i++)
            {
                Regex regex = new Regex(patterns[i]);
                Match match = regex.Match(sentance);
                if (match.Success && (match.Index < 3))
                {
                    IsAns = true;
                }
            }
            return IsAns;
        }
        public bool IsMachingWithRightQuestion(string sentance)
        {
            bool IsAns = false;
            string [] patterns = new string[] {@"Raspunsul corect este", @"Răspuns corect:"};
            for (int i = 0; i < patterns.Length; i++)
            {
                Regex regex = new Regex(patterns[i]);
                Match match = regex.Match(sentance);
                if (match.Success)
                {
                    IsAns = true;
                }
            }
            return IsAns;
        }

        public int GetQuestionId(string questionText)
        {
            string[] numbers = Regex.Split(questionText, @"\D+");
            return Convert.ToInt32(numbers[0].Trim());
        }
        
        public string ClearSentance(string sentance)
        {
            string[] numbers = Regex.Split(sentance, @"\D+");
            sentance = sentance.Replace(numbers[0], "");

            return Regex.Replace(sentance, @"[^\w\s^(țâîăÂÎĂȘȚș)]", "");
        }
    }
}

