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


namespace AnswerSelection
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

                    foreach(string paragraph in paragraphs)
                    {
                        string[] splited = Regex.Split(paragraph, @"\n \n");
                        foreach(string split in splited)
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

                foreach(Question question in QuestionList)
                {
                    AnswerProcess answerProcess = new AnswerProcess(question.AnswerList, question.LanguageId);
                 
                    List<BackgroundDocument> passageRetrived = new List<BackgroundDocument>();

                    if (!string.IsNullOrEmpty(question.QuestionAfterProcess))
                    {
                        passageRetrived = indexing.Search(question.QuestionAfterProcess.ToLower(), searchField);
                        if(passageRetrived.Count > 0)
                        {
                            List<Answer> answers = CheckAnswers(question.AnswerList, passageRetrived,question.Focus);

                            if(multipleChoises == false)
                            {
                                answers = answers.OrderBy(o => o.Score).ToList();
                                //bool flag = true;
                                //if (answers[0].Score > 3)
                                //{
                                //    for (int index = 1; index < answers.Count(); index++)
                                //    {

                                //        if (answers[index] == answers[0])
                                //        {
                                //            flag = false;
                                //        }
                                //    }
                                //    if (flag == true)
                                //    {
                                //        answers[0].IsCorrectAnswer = true;
                                //        question.AnswerList = answers;
                                //        question.IsAnswered = true;
                                //        NrOfQuestionAnswered += 1;

                                //    }
                                //}
                            }

                            //if (multipleChoises == false && oneAnswerSelected(answers))
                            //{
                            //    question.AnswerList = answers;
                            //    question.IsAnswered = true;
                            //    NrOfQuestionAnswered += 1;
                                
                            //}
                            else
                            {
                                if (MoreThanOneAnswerSelected(answers) == true)
                                {
                                    List<Answer> answersProcessed = ChoseCorectAnswer(answers, question.Focus, indexing);
                                }
                            }
                        }
                    }
                    // question.AnswerList = answerProcess.ProcessAnswers();
                    if (!string.IsNullOrEmpty(question.Focus))
                    {
                         passageRetrived = GetPassageByFocusQuery(indexing, question.Focus.ToLower(), searchField, methodAnd);
                    }

                    if (passageRetrived.Count <= 0 || passageRetrived[0].Score < 0.700)
                    {
                        if (question.KeyWords.Count > 0)
                        {
                            passageRetrived = GetPassageByKeywordsQuery(indexing, question.KeyWords, searchField, methodAnd);

                            if (passageRetrived.Count() < 0 || passageRetrived[0].Score < 0.500)
                            {
                                
                            }
                        }
                    }
                    else
                    {
                        
                    }

                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
        }

        private List<Answer> ChoseCorectAnswer(List<Answer> answers, string p, LuceneService indexing)
        {
            foreach(Answer answer in answers)
            {
                if(answer.IsCorrectAnswer == true)
                {

                }
            }
            return null;
        }

        private bool MoreThanOneAnswerSelected(List<Answer> list)
        {
            int index = 0;
            foreach(Answer answer in list)
            {
                if(answer.IsCorrectAnswer)
                {
                    index += 1;
                }
            }

            if(index > 1)
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
            
            foreach(BackgroundDocument retrievedDocument in passageRetrived)
            {
                List<string> wordsFindedInSen = new List<string>();
                foreach(Answer answer in list)
                {
                    Regex replacer2 = new Regex(@"\b(?:[a-zA-Z]\.\s|[a-zA-Z]\)\s)\b");
                    answer.CleareText = replacer2.Replace(answer.Text, "");
                    answer.CleareText = Regex.Replace(answer.CleareText, @"[^\w\s^ă^î^â^ș^ț^Ă^Î^Â^Ș^Ț]", "");
                    Regex regex = new Regex("(" + answer.CleareText + ")");
                    string text = Regex.Replace(retrievedDocument.Paragraph, @"[^\w\s^ă^î^â^ș^ț^Ă^Î^Â^Ș^Ț]", " ");
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
                    if(match.Success)
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


                        //cred ca aici o sa trebuiasa ca verificam si intrebarea daca n/2 apar in text
                        //
                        string[] wordsFromAnswer = answer.TextLematized.Split(' ');
                        string[] sentences = Regex.Split(text, @"([\.!\?])\s+");
                        int numberOfWordsFinded = 0;
                        int maxPerSent = 0;
                        foreach(string sentence in sentences)
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
                                                patternWord = patternWord.Substring(0, patternWord.Length - 3);
                                            }
                                            else if (patternWord.Length > 3)
                                            {
                                                patternWord = patternWord.Substring(0, patternWord.Length - 2);
                                            }

                                            Regex pattern = new Regex("(" + patternWord + ")");
                                            Match matchpatt = pattern.Match(sentence.ToLower());
                                            if (matchpatt.Success)
                                            {
                                                if (wordsFindedInSen.Contains(word) == false)
                                                {
                                                    numberOfWordsFinded += 1;
                                                    wordsFindedInSen.Add(word);
                                                }
                                            }
                                            if (language == Languages.English)
                                            {
                                                BigHugeLabs bigHugeLabs = new BigHugeLabs();
                                                string response = bigHugeLabs.CallApi(word.Trim());
                                                
                                                Rootobject responseObject = DeserializeJson.Deserialize(response);
                                                if (ProcessSynonyms(sentence, responseObject) > 0)
                                                {
                                                    wordsFindedInSen.Add(word);
                                                }
                                                numberOfWordsFinded += ProcessSynonyms(sentence, responseObject);
                                            }

                                        }
                                    }

                                    if(answer.MaxNrWFinded == null || answer.MaxNrWFinded < numberOfWordsFinded)
                                    {
                                        answer.MaxNrWFinded = numberOfWordsFinded;
                                        answer.Paragraph = sentence;
                                    }
                                }
                            }

             //for sent
                            if(numberOfWordsFinded > maxPerSent)
                            {
                                maxPerSent = numberOfWordsFinded;
                            }
                            if (numberOfWordsFinded > processedWords/2 + 1)
                            {
                                answer.IsCorrectAnswer = true;
                                answer.Passage.Add(text);
                            }
                        }
                    }
                }
            }
            return list;
        }

        private int  ProcessSynonyms(string sentence, Rootobject responseObject)
        {
            int numberOfWordsFinded = 0;
            if(responseObject.adjective != null)
            {
                if(responseObject.adjective.syn != null)
                {
                    foreach(string synonim in responseObject.adjective.syn )
                    {
                        Regex pattern = new Regex("(" + synonim + ")");
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
                    foreach (string synonim in responseObject.noun.syn)
                    {
                        Regex pattern = new Regex("(" + synonim + ")");
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
                    foreach (string synonim in responseObject.verb.syn)
                    {
                        Regex pattern = new Regex("(" + synonim + ")");
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
            sentence = sentence.ToLower();
            sentence = Regex.Replace(sentence, @"[^\w\s^ă^î^â^ș^ț^Ă^Î^Â^Ș^Ț]", " ");
            sentence = sentence.Trim();
            if (!string.IsNullOrEmpty(sentence))
            {
                string[] focusWords = focus.Split(' ');
                numberOfWordsThatNeedToBeFound = focusWords.Length / 2;

                foreach (string focuss in focusWords)
                {
                    string f = string.Empty;
                    if (focuss.Length > 6)
                    {
                        f = focuss.Substring(0, focuss.Length - 3);
                    }
                    else if (focuss.Length > 3)
                    {
                        f = focuss.Substring(0, focuss.Length - 2);
                    }

                    Regex pattern = new Regex("(" + f + ")");
                    Match match = pattern.Match(sentence.ToLower());
                    if (match.Success)
                    {
                        numberOfWordsFinded += 1;
                    }
                    else if (language == Languages.English)
                    {
                        BigHugeLabs bigHugeLabs = new BigHugeLabs();
                        string response = bigHugeLabs.CallApi(focuss.Trim());
                        Rootobject responseObject = DeserializeJson.Deserialize(response);
                        numberOfWordsFinded += ProcessSynonyms(sentence, responseObject);
                    }
                }
            }
            if(numberOfWordsFinded == numberOfWordsThatNeedToBeFound)
            {
                return true;
            }
            return false;
        }
        private List<BackgroundDocument> GetPassageByKeywordsQuery(LuceneService indexing, List<Word> keyWordList, string searchField, string method)
        {
            string query = string.Empty;
            if(keyWordList.Count() == 1)
            {
                query = keyWordList[0].Value.ToLower();
            }
            else if (keyWordList.Count() == 2)
            {
                query = keyWordList[0].Value.ToLower() + method + keyWordList[1].Value.ToLower();
            }
            else
            {
                for(int i =0; i< keyWordList.Count  ; i++)
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
                if(!string.IsNullOrEmpty(focusWords[0]))
                {
                     query = focusWords[0];
                }
            }
            else if (focusWords.Length == 2)
            {
                if(!string.IsNullOrEmpty(focusWords[0]) && !string.IsNullOrEmpty(focusWords[1]))
                {
                     query = focusWords[0] + method + focusWords[1];
                }
            }
            else
            {
                for(int i = 0; i < focusWords.Length ; i++)
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
