using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestionAnalisys;
using TextProcessing;
using System.Text.RegularExpressions;
using IndexerLucene;
using APIs;
using DeserializeProject;
using System.Web;

namespace AnswerEvaluation
{
    public class AnswerExtract
    {
        private List<BackgroundDocument> Documents;
        private List<Question> QuestionList;
        private int NrOfQuestionAnswered = 0;

        string searchField = "ProcessedText";
        private string methodAnd = " AND ";
        private string methodOR = " OR ";
        private Languages language;
        private bool multipleChoises;
        public AnswerExtract(List<BackgroundDocument> documents, List<Question> questionList, bool multipleChoises)
        {
            Documents = new List<BackgroundDocument>();
            Documents = documents;
            QuestionList = questionList;
            language = questionList[0].LanguageId;
            multipleChoises = multipleChoises;
        }

        public void AnswerAnalysis()
        {
            try
            {
                TextProcessing.PdfProcessing backgroundText = new TextProcessing.PdfProcessing(Documents);
                List<TextDocument> DocumentList = backgroundText.GetAllText();
                List<string> collection = new List<string>();

                for (int i = 0; i < DocumentList.Count; i++)
                {
                    string[] paragraphs = Regex.Split(DocumentList[i].Text.Trim(), @"\r\n");

                    foreach (string paragraph in paragraphs)
                    {
                        string[] splited = Regex.Split(paragraph, @"\n \n");
                        foreach (string split in splited)
                        {
                            Regex regex = new Regex(@"([A-Za-z])\w+");
                            Match match = regex.Match(split);
                            if (match.Success)
                            {
                                collection.Add(split);
                            }
                        }
                    }

                    paragraphs = collection.ToArray();

                    for (int j = 0; j < paragraphs.Length; j++)
                    {

                        if ((!string.IsNullOrWhiteSpace(paragraphs[j]) || !string.IsNullOrEmpty(paragraphs[j])) && paragraphs[j] != " ")
                        {
                            paragraphs[j] = paragraphs[j].Replace('\n', ' ');
                            paragraphs[j] = paragraphs[j].Trim();
                            paragraphs[j] = Regex.Replace(paragraphs[j], @"\s+", " ");
                        }
                    }
                    Documents[i].Paragraphs = paragraphs;

                    string lowercaseText = DocumentList[i].Text.Trim().ToLower();

                    Regex replacer;
                    if (language == Languages.English)
                    {
                        replacer = new Regex(StopWords.SWE);
                    }
                    else
                    {
                        replacer = new Regex(StopWords.SWR);
                    }

                    lowercaseText = replacer.Replace(lowercaseText, "");

                    string[] lowercaseParagraphs = Regex.Split(lowercaseText, @"\n\n");
                    Documents[i].ProcessedParagraphs = lowercaseParagraphs;
                }

                LuceneService indexing = new LuceneService();
                indexing.BuildIndex(Documents);

                foreach (Question question in QuestionList)
                {
                    AnswerProcess answerProcess = new AnswerProcess(question.AnswerList, question.LanguageId);

                    List<BackgroundDocument> passageRetrived = new List<BackgroundDocument>();

                    if (!string.IsNullOrEmpty(question.QuestionAfterProcess))
                    {
                        passageRetrived = indexing.Search(question.QuestionAfterProcess.ToLower(), searchField);
                        if (passageRetrived.Count > 0)
                        {
                            string focus;
                            if (string.IsNullOrEmpty(question.Focus))
                            {
                                focus = question.QuestionLemmatized;
                            }
                            else
                            {
                                focus = question.Focus;
                            }

                            List<Answer> answers = CheckAnswers(question.AnswerList, passageRetrived, focus);

                            if (multipleChoises == false)
                            {
                                List<double> Scores = new List<double>();
                                List<Answer> correctAnswers = new List<Answer>();
                                foreach (Answer ans in answers)
                                {
                                    ans.Score = 0;
                                    if (ans.IsCorrectAnswer == true)
                                    {
                                        correctAnswers.Add(ans);
                                        break;
                                    }
                                    else if (ans.MaxNrWFinded > 0)
                                    {
                                        ans.Score = (double)ans.MaxNrWFinded / (double)ans.NrWNeedToBeFound;
                                    }
                                    Scores.Add(ans.Score);

                                }

                                if (correctAnswers.Count() > 0)
                                {
                                    question.CorrectAnswers = correctAnswers;
                                    question.IsAnswered = true;
                                    NrOfQuestionAnswered += 1;
                                }
                                else
                                {
                                    answers = answers.OrderByDescending(o => o.Score).ToList();
                                    question.AnswerList = answers;
                                    if (Scores.Count() > 0)
                                    {
                                        bool isUnique = Scores.Distinct().Count() == Scores.Count();
                                        if (isUnique == false)
                                        {
                                            if (answers[0].Score > answers[1].Score)
                                            {
                                                if ((answers[0].Score - answers[1].Score) < 0.07)
                                                {
                                                    answers[1].IsCorrectAnswer = true;
                                                    correctAnswers.Add(answers[1]);
                                                }

                                                answers[0].IsCorrectAnswer = true;

                                                correctAnswers.Add(answers[0]);
                                            }
                                            if (correctAnswers.Count() > 0)
                                            {
                                                question.CorrectAnswers = correctAnswers;
                                                question.IsAnswered = true;
                                                NrOfQuestionAnswered += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        static public string RemoveDuplicateWords(string sentence)
        {
            var d = new Dictionary<string, bool>();
            StringBuilder b = new StringBuilder();
            string[] words = sentence.Split(new char[] { ' ', ',', ';', '.' },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string current in words)
            {
                string lower = current.ToLower();
                if (lower.Length > 6)
                {
                    lower = lower.Substring(0, lower.Length - 2);
                }
                else if (lower.Length > 4)
                {
                    lower = lower.Substring(0, lower.Length - 1);
                }
            
                if (!d.ContainsKey(lower))
                {
                    b.Append(current).Append(' ');
                    d.Add(lower, true);
                }
            }
     
            return b.ToString().Trim();
        }
        private List<Answer> ChoseCorectAnswer(List<Answer> answers, string p, LuceneService indexing)
        {
            foreach (Answer answer in answers)
            {
                if (answer.IsCorrectAnswer == true)
                {

                }
            }
            return null;
        }

        private bool MoreThanOneAnswerSelected(List<Answer> list)
        {
            int index = 0;
            foreach (Answer answer in list)
            {
                if (answer.IsCorrectAnswer)
                {
                    index += 1;
                }
            }

            if (index > 1)
            {
                return true;
            }
            return false;
        }

        private bool oneAnswerSelected(List<Answer> list)
        {
            int index = 0;
            foreach (Answer answer in list)
            {
                if (answer.IsCorrectAnswer)
                {
                    index += 1;
                }
            }

            if (index == 1)
            {
                return true;
            }
            return false;
        }

        private List<Answer> CheckAnswers(List<Answer> list, List<BackgroundDocument> passageRetrived, string Focus)
        {

            foreach (BackgroundDocument retrievedDocument in passageRetrived)
            {
                List<string> wordsFindedInSen = new List<string>();
                foreach (Answer answer in list)
                {
                    
                    Regex replacer2 = new Regex(@"\b(?:[a-zA-Z]\.\s|[a-zA-Z]\)\s)\b");
                    answer.CleareText = replacer2.Replace(answer.Text, ""); //clear matket anser a. a)

                    answer.CleareText = Regex.Replace(answer.CleareText, @"[^\w\s^ă^î^â^ș^ț^Ă^Î^Â^Ș^Ț]", ""); //replace punctuation

                    Regex regex = new Regex(@"\b" + answer.CleareText + @"\S*\b");

                    string text = Regex.Replace(retrievedDocument.Paragraph, @"[^\w\s^ă^î^â^ș^ț^Ă^Î^Â^Ș^Ț]", " ");
                    answer.CleareText = RemoveDuplicateWords(answer.CleareText);
                    if (text.Contains('('))
                    {
                        text = text.Remove('(', ' ');
                    }
                    if (text.Contains(')'))
                    {
                        text = text.Replace(')', ' ');
                    }
                    if (answer.CleareText.Contains('('))
                    {
                        answer.CleareText = answer.CleareText.Replace('(', ' ');
                    }
                    if (answer.CleareText.Contains(')'))
                    {
                        answer.CleareText = answer.CleareText.Replace(')', ' ');
                    }

                    Match match = regex.Match(text);
                    if (match.Success)
                    {
                        answer.IsCorrectAnswer = true;
                        answer.Passage.Add(text);
                    }
                    else
                    {
                        Regex replacer;

                        if (language == Languages.English)
                        {
                            replacer = new Regex(StopWords.SWE);
                        }
                        else
                        {
                            replacer = new Regex(StopWords.SWR);
                        }

                        answer.TextLematized = replacer.Replace(answer.CleareText, "");
                        answer.TextLematized = answer.TextLematized.Trim();
                        text = replacer.Replace(retrievedDocument.Paragraph, "");
                        text = text.Trim();

                        string[] wordsFromAnswer = answer.TextLematized.Split(' ');
                        string[] sentences = Regex.Split(text, @"([\.!\?])\s+");
                        int numberOfWordsFinded = 0;
                        int maxPerSent = 0;
                        foreach (string sentence in sentences)
                        {
                            numberOfWordsFinded = 0;
                            int processedWords = 0;

                            if (IsMatchWithFocus(sentence, Focus) == true)
                            {
                                Regex re = new Regex(@"([A-Za-z])\w+");
                                Match matchs = re.Match(sentence);

                                if (matchs.Success)
                                {

                                    foreach (string word in wordsFromAnswer)
                                    {
                                        Regex regexWORD = new Regex(@"([A-Za-z])\w+");
                                        Match matchw = regexWORD.Match(word);
                                        if (matchw.Success)
                                        {
                                            processedWords += 1;
                                            // try find word in sentence
                                            string patternWord = word.ToLower();
                                            if (patternWord.Length > 6)
                                            {
                                                patternWord = patternWord.Substring(0, patternWord.Length - 2);
                                            }
                                            else if (patternWord.Length > 4)
                                            {
                                                patternWord = patternWord.Substring(0, patternWord.Length - 1);
                                            }

                                            Regex pattern = new Regex(@"\b" + patternWord + @"\S*\b");
                                            Match matchpatt = pattern.Match(sentence.ToLower());
                                            if (matchpatt.Success)
                                            {
                                                numberOfWordsFinded += 1;

                                                if (wordsFindedInSen.Contains(word) == false)
                                                {
                                                    wordsFindedInSen.Add(word);
                                                }
                                            }
                                            else if (language == Languages.English)
                                            {
                                                BigHugeLabs bigHugeLabs = new BigHugeLabs();
                                                Rootobject responseObject;
                                                string response = bigHugeLabs.CallApi(word.Trim());
                                                if (!string.IsNullOrEmpty(response))
                                                {

                                                    responseObject = DeserializeJson.Deserialize(response);

                                                    if (ProcessSynonyms(sentence, responseObject) > 0)
                                                    {
                                                        wordsFindedInSen.Add(word);
                                                    }

                                                    numberOfWordsFinded += ProcessSynonyms(sentence, responseObject);
                                                }
                                            }

                                        }
                                    }
                                    if (answer.MaxNrWFinded == null || answer.MaxNrWFinded < numberOfWordsFinded)
                                    {
                                        answer.MaxNrWFinded = numberOfWordsFinded;
                                        answer.Paragraph = sentence;
                                        answer.NrWNeedToBeFound = processedWords;
                                        answer.Passage.Add(retrievedDocument.Paragraph);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        private int ProcessSynonyms(string sentence, Rootobject responseObject)
        {
            int numberOfWordsFinded = 0;
            if (responseObject.adjective != null)
            {
                if (responseObject.adjective.syn != null)
                {
                    for (int i = 0; i <= responseObject.adjective.syn.Length / 3 + 1; i++)
                    {
                        Regex pattern = new Regex("\b" + responseObject.adjective.syn[i] + "\b");
                        Match match = pattern.Match(sentence.ToLower());
                        if (match.Success)
                        {
                            numberOfWordsFinded += 1;
                        }
                    }
                }
            }

            if (responseObject.noun != null)
            {
                if (responseObject.noun.syn != null)
                {
                    for (int i = 0; i <= responseObject.noun.syn.Length / 3 + 1; i++)
                    {
                        Regex pattern = new Regex("\b" + responseObject.noun.syn[i] + "\b");
                        Match match = pattern.Match(sentence.ToLower());
                        if (match.Success)
                        {
                            numberOfWordsFinded += 1;
                        }
                    }
                }
            }

            if (responseObject.verb != null)
            {
                if (responseObject.verb.syn != null)
                {
                    for (int i = 0; i <= responseObject.verb.syn.Length / 3 + 1; i++)
                    {
                        Regex pattern = new Regex("\b" + responseObject.verb.syn[i] + "\b");
                        Match match = pattern.Match(sentence.ToLower());
                        if (match.Success)
                        {
                            numberOfWordsFinded += 1;
                        }
                    }
                }
            }

            return numberOfWordsFinded;
        }

        private bool IsMatchWithFocus(string sentence, string focus)
        {
            int numberOfWordsThatNeedToBeFound = 0;
            int numberOfWordsFinded = 0;
            int nr = 0;
            sentence = sentence.ToLower();
            sentence = Regex.Replace(sentence, @"[^\w\s^ă^î^â^ș^ț^Ă^Î^Â^Ș^Ț]", " ");
            sentence = sentence.Trim();
            if (!string.IsNullOrEmpty(sentence))
            {
                string[] focusWords = focus.Split(' ');
               

                foreach (string focuss in focusWords)
                {
                    if (!string.IsNullOrEmpty(focuss))
                    {
                        nr += 1;
                        string f = string.Empty;
                        if (focuss.Length > 6)
                        {
                            f = focuss.Substring(0, focuss.Length - 3);
                        }
                        else if (focuss.Length > 4)
                        {
                            f = focuss.Substring(0, focuss.Length - 2);
                        }

                        Regex pattern = new Regex(@"\b" + f + @"\S*\b");
                        Match match = pattern.Match(sentence.ToLower());
                        if (match.Success)
                        {
                            numberOfWordsFinded += 1;
                        }
                        else if (language == Languages.English)
                        {

                            BigHugeLabs bigHugeLabs = new BigHugeLabs();
                            string response = bigHugeLabs.CallApi(focuss.Trim());
                            if (!string.IsNullOrEmpty(response))
                            {
                                Rootobject responseObject = DeserializeJson.Deserialize(response);
                                numberOfWordsFinded += ProcessSynonyms(sentence, responseObject);
                            }
                        }
                    }
                }
                if (nr > 1)
                {
                    numberOfWordsThatNeedToBeFound = nr / 2 + 1;
                }
                else if(nr == 1 )
                {
                    numberOfWordsThatNeedToBeFound = 1;
                }
            }
            if ((numberOfWordsFinded >= numberOfWordsThatNeedToBeFound) && numberOfWordsThatNeedToBeFound != 0 && numberOfWordsFinded !=0)
            {
                return true;
            }
            return false;
        }
        private List<BackgroundDocument> GetPassageByKeywordsQuery(LuceneService indexing, List<Word> keyWordList, string searchField, string method)
        {
            string query = string.Empty;
            if (keyWordList.Count() == 1)
            {
                query = keyWordList[0].Value.ToLower();
            }
            else if (keyWordList.Count() == 2)
            {
                query = keyWordList[0].Value.ToLower() + method + keyWordList[1].Value.ToLower();
            }
            else
            {
                for (int i = 0; i < keyWordList.Count; i++)
                {
                    if (i < keyWordList.Count - 1)
                    {
                        query += keyWordList[i].Value.ToLower() + method;
                    }
                    else
                    {
                        query += keyWordList[i].Value.ToLower();
                    }
                }
            }
            return indexing.Search(query, searchField);
        }

        private List<BackgroundDocument> GetPassageByFocusQuery(LuceneService indexing, string focus, string searchField, string method)
        {
            focus = focus.Trim().Replace(" ", "|");
            string[] focusWords = focus.Split('|');
            string query = string.Empty;
            if (focusWords.Length < 2 && focusWords.Length != 0)
            {
                if (!string.IsNullOrEmpty(focusWords[0]))
                {
                    query = focusWords[0];
                }
            }
            else if (focusWords.Length == 2)
            {
                if (!string.IsNullOrEmpty(focusWords[0]) && !string.IsNullOrEmpty(focusWords[1]))
                {
                    query = focusWords[0] + method + focusWords[1];
                }
            }
            else
            {
                for (int i = 0; i < focusWords.Length; i++)
                {
                    if (i < focusWords.Length - 1)
                    {
                        if (!string.IsNullOrEmpty(focusWords[i]))
                        {
                            query += focusWords[i] + method;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(focusWords[i]))
                        {
                            query += focusWords[i];
                        }
                    }
                }
            }

            return indexing.Search(query, searchField);
        }
    }
}
