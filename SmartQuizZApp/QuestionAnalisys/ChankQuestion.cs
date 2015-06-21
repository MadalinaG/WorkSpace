using QuestionAnalisys.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuestionAnalisys
{
    public class ChankQuestion
    {
        private string xml;
        private Languages language;
        public List<Word> KeyWords;
        public string Focus;
        public QuestionType type;
        public ChankQuestion(string xml, Languages language)
        {
            this.xml = xml;
            this.language = language;
            KeyWords = new List<Word>();
        }

        public Sentance CreateSentence()
        {
            Sentance sentance = new Sentance();
            sentance.Words = new List<Word>();
            try
            {
                if(!string.IsNullOrEmpty(xml))
                {
                    DTO.DOCUMENT doc = HttpHandlers.Deserialize<DTO.DOCUMENT>(xml);

                    XDocument xmlDocument = XDocument.Parse(xml);
                    XElement firstElement = xmlDocument.Descendants("DOCUMENT").FirstOrDefault();
                    XElement pElement = firstElement.Element("P");
                    XElement sElement = pElement.Element("S");
                    XElement[] wElements = sElement.Elements("W").ToArray();
                    XElement[] npElements = sElement.Elements("NP").ToArray();

                    foreach (XElement npelement in wElements)
                    {
                        sentance.Words.Add(WordProperties(npelement, false));
                    }

                    foreach (XElement element in npElements)
                    {
                        XElement[] npWElement = element.Elements("W").ToArray();
                        XElement headElement = element.Elements("HEAD").FirstOrDefault();
                        XElement nestedNPElement = element.Elements("NP").FirstOrDefault();

                        if (npWElement.Count() > 0)
                        {
                            foreach (XElement npelement in npWElement)
                            {
                                sentance.Words.Add(WordProperties(npelement, false));
                            }
                        }

                        if(headElement != null)
                        {
                            XElement[] headWElements = headElement.Elements("W").ToArray();
                            foreach (XElement headelement in headWElements)
                            {
                                sentance.Words.Add(WordProperties(headelement, true));
                            }
                        }

                        if(nestedNPElement != null)
                        {
                            XElement[] nestedNpWElement = nestedNPElement.Elements("W").ToArray();
                            XElement nestedHeadElement = nestedNPElement.Elements("HEAD").FirstOrDefault();
                            XElement NP = nestedNPElement.Elements("NP").FirstOrDefault();

                            if (nestedNpWElement.Count() > 0)
                            {
                                foreach (XElement nestedelement in nestedNpWElement)
                                {
                                    sentance.Words.Add(WordProperties(nestedelement, false));
                                }
                            }

                            if (nestedHeadElement != null)
                            {
                                XElement[] headWElements = nestedHeadElement.Elements("W").ToArray();
                                foreach (XElement headWelement in headWElements)
                                {
                                    sentance.Words.Add(WordProperties(headWelement, true));
                                }
                            }

                            if(NP != null)
                            {
                                XElement[] Wnest = NP.Elements("W").ToArray();
                                XElement headE = NP.Elements("HEAD").FirstOrDefault();
                                
                                if(Wnest != null)
                                {
                                    foreach(XElement el in Wnest)
                                    {
                                        sentance.Words.Add(WordProperties(el, false));
                                    }
                                }

                                if(headE != null)
                                {
                                    XElement w = headE.Elements("W").FirstOrDefault();
                                    sentance.Words.Add(WordProperties(w, true));
                                }
                            }
                        }
                    }

                }
                else
                {
                    throw new Exception(string.Format("'Xml is not valid. Encountered: {1}.", xml));
                }
            }
            catch (Exception Ex)
            {

            }

            if(sentance.Words.Count() > 0 )
            {
                sentance.Words = sentance.Words.OrderBy(o => o.offset).ToList();
            }
            return sentance;
            
        }

        private Word WordProperties(XElement element, bool isNp)
        {
            Word word = new Word();
            word.LEMMA = element.Attribute("LEMMA").Value;
            word.POS = GetPartOfSpeech(element.Attribute("POS").Value);
            word.offset = Convert.ToInt32(element.Attribute("offset").Value);
            word.Value = element.Value;

            if(isNp == true)
            {
                word.KeyWord = true;
            }
            else
            {
                word.KeyWord = false;
            }
            return word;
        }

        private PartOfSpeech GetPartOfSpeech(string partOfSpeech)
        {
            PartOfSpeech pos = PartOfSpeech.UNRECOGNIZED;
            if (language == Languages.Romanian)
            {
                if (!string.IsNullOrEmpty(partOfSpeech))
                {
                    if (partOfSpeech == "VERB")
                    {
                        pos = PartOfSpeech.VERB;
                    }
                    if (partOfSpeech == "NOUN")
                    {
                        pos = PartOfSpeech.NOUN;
                    }
                    if (partOfSpeech == "ADJECTIVE")
                    {
                        pos = PartOfSpeech.ADJECTIVE;
                    }
                    if (partOfSpeech == "CONJUNCTION")
                    {
                        pos = PartOfSpeech.CONJUNCTION;
                    }
                    if (partOfSpeech == "ADPOSITION")
                    {
                        pos = PartOfSpeech.ADPOSITION;
                    }
                    if (partOfSpeech == "ARTICLE")
                    {
                        pos = PartOfSpeech.ARTICLE;
                    }
                    if (partOfSpeech == "ADVERB")
                    {
                        pos = PartOfSpeech.ADVERB;
                    }
                    if (partOfSpeech == "DETERMINER")
                    {
                        pos = PartOfSpeech.DETERMINER;
                    }
                    if (partOfSpeech == "PRONOUN")
                    {
                        pos = PartOfSpeech.PRONOUN;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(partOfSpeech))
                {
                    if (partOfSpeech == "VB" || partOfSpeech == "VBD" || partOfSpeech == "VBG" || partOfSpeech == "VBN" || partOfSpeech == "VBP" || partOfSpeech == "VBZ")
                    {
                        pos = PartOfSpeech.VERB;
                    }
                    if (partOfSpeech == "NN" || partOfSpeech == "NNS" || partOfSpeech == "NNP" || partOfSpeech == "NNPS")
                    {
                        pos = PartOfSpeech.NOUN;
                    }
                    if (partOfSpeech == "JJ" || partOfSpeech == "JJR" || partOfSpeech == "JJS")
                    {
                        pos = PartOfSpeech.ADJECTIVE;
                    }
                    if (partOfSpeech == "CC")
                    {
                        pos = PartOfSpeech.CONJUNCTION;
                    }
                    if (partOfSpeech == "RB" || partOfSpeech == "RBR" || partOfSpeech == "RBS")
                    {
                        pos = PartOfSpeech.ADVERB;
                    }
                    if (partOfSpeech == "DT")
                    {
                        pos = PartOfSpeech.DETERMINER;
                    }
                    if (partOfSpeech == "PRP" || partOfSpeech == "PRP$")
                    {
                        pos = PartOfSpeech.PRONOUN;
                    }
                    if (partOfSpeech == "CD")
                    {
                        pos = PartOfSpeech.NUMERAL;
                    }
                    if (partOfSpeech == "IN")
                    {
                        pos = PartOfSpeech.PREPOSITION;
                    }
                    if (partOfSpeech == "MD")
                    {
                        pos = PartOfSpeech.MODAL;
                    }
                    if (partOfSpeech == "PDT")
                    {
                        pos = PartOfSpeech.PREDETERMINER;
                    }
                    if (partOfSpeech == "RP")
                    {
                        pos = PartOfSpeech.PARTICLE;
                    }
                    if (partOfSpeech == "UH")
                    {
                        pos = PartOfSpeech.INTERJECTION;
                    }
                    if (partOfSpeech == "WDT")
                    {
                        pos = PartOfSpeech.WHDETERMINER;
                    }
                    if (partOfSpeech == "WP" || partOfSpeech == "WP$")
                    {
                        pos = PartOfSpeech.WHPRONOUN;
                    }
                    if (partOfSpeech == "WRB")
                    {
                        pos = PartOfSpeech.WHADVERB;
                    }
                }
            }
            return pos;
        }

        public Sentance GetLemma()
        {
            Sentance question = new Sentance();

            if(!string.IsNullOrEmpty(xml))
            {
                question.Words = new List<Word>();
                XDocument xmlDocument = XDocument.Parse(xml);
                XElement firstElement = xmlDocument.Descendants("seg").FirstOrDefault();
                XElement sElement = firstElement.Element("s");
                XElement[] wElements = sElement.Elements("w").ToArray();
                foreach(var element in wElements)
                {
                    Word wordFromQuestion = new Word();
                    wordFromQuestion.LEMMA = element.Attribute("lemma").Value;
                    wordFromQuestion.Value = element.Value;
                    wordFromQuestion.POS = GETPSpeach(element.Attribute("ana").Value);
                    if (element.Attribute("chunk") != null)
                    {
                        wordFromQuestion.KeyWord = IsNP(element.Attribute("chunk").Value);
                        wordFromQuestion.offset = GetOffset(wordFromQuestion.KeyWord, element.Attribute("chunk").Value);
                    }
                    question.Words.Add(wordFromQuestion);

                    if (char.IsUpper(wordFromQuestion.Value[0]))
                    {
                        Focus = Focus + " " + wordFromQuestion.Value;
                    }
                }
            }
            return question;
        }

        private int GetOffset(bool isNp, string NP)
        {
            int offset = 0;
            if(isNp)
            {
                int start = NP.IndexOf("NP#");
                char off = NP[start + 3];
                offset = Convert.ToInt32(off);
            }
            return offset;
        }

        private bool IsNP(string p)
        {
            if(p.Contains("NP"))
            {
                return true;
            }
            return false;
        }

        private PartOfSpeech GETPSpeach(string p)
        {
            PartOfSpeech pos = PartOfSpeech.UNRECOGNIZED;
            if(!string.IsNullOrEmpty(p))
            {
                char c = p[0];
                switch(c)
                {
                    case 'A':
                        pos = PartOfSpeech.ADJECTIVE;
                        break;
                    case 'C':
                        pos = PartOfSpeech.CONJUNCTION;
                        break;
                    case 'Y':
                        pos = PartOfSpeech.ABREVIATION;
                        break;
                    case 'V':
                        pos = PartOfSpeech.VERB;
                        break;
                    case 'I':
                        pos = PartOfSpeech.INTERJECTION;
                        break;
                    case 'M':
                        pos = PartOfSpeech.NUMERAL;
                        break;
                    case 'N':
                        pos = PartOfSpeech.NOUN;
                        break;
                    case 'P':
                        pos = PartOfSpeech.PRONOUN;
                        break;
                    case 'Q':
                        pos = PartOfSpeech.PARTICLE;
                        break;
                    case 'R':
                        pos = PartOfSpeech.ADVERB;
                        break;
                    case 'S':
                        pos = PartOfSpeech.ADPOSITION;
                        break;
                    case 'D':
                        pos = PartOfSpeech.ARTICLE;
                        break;
                    default:
                        pos = PartOfSpeech.UNRECOGNIZED;
                        break;
                }
            }
            return pos;
        }

        public List<KeyWord> GETKeyWordsQ()
        {
            List<KeyWord> KeyWordList = new List<KeyWord>();
            if (!string.IsNullOrEmpty(xml))
            {
                XDocument xmlDocument = XDocument.Parse(xml);
                XElement keywordsElement = xmlDocument.Descendants("keywords").FirstOrDefault();
                XElement[] keywordElements = keywordsElement.Elements("keyword").ToArray();
                foreach(XElement element in keywordElements)
                {
                    XElement relevance = element.Element("relevance");
                    XElement text = element.Element("text");
                    KeyWord key = new KeyWord();
                    key.Relevance = Convert.ToDouble(relevance.Value);
                    key.Text = text.Value;
                    KeyWordList.Add(key);
                }
            }
            return KeyWordList;
        }

        public Sentance GetQuestionProcessed(DOCUMENT document)
        {
            Sentance sentance = new Sentance();
            sentance.Words = new List<Word>();
            
            foreach(S s in document.Part.S)
            {
                if(s.W != null)
                {
                    foreach(W w in s.W)
                    {
                        Word word = new Word();
                        word.KeyWord = false;
                        word.LEMMA = w.LEMMA;
                        word.offset = w.offset;
                        word.Value = w.Value;
                        word.POS = GetPartOfSpeech(w.POS);
                        word.Score = 25;
                        sentance.Words.Add(word);
                    }
                }

                if(s.NP != null)
                {
                    foreach(NP np in s.NP)
                    {
                        if(np.W != null)
                        {
                            if (np.TYPE == "PERSON")
                            {
                                type = QuestionType.FactoidPerson; 
                            }

                            if (np.TYPE == "LOCATION")
                            {
                                type = QuestionType.FactoidLocation;
                            }

                            foreach (W w in np.W)
                            {
                                Word word = new Word();
                                word.KeyWord = true;
                                word.LEMMA = w.LEMMA;
                                word.offset = w.offset;
                                word.Value = w.Value;
                                word.POS = GetPartOfSpeech(w.POS);
                                word.Score = 50;
                                sentance.Words.Add(word);

                                if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                {
                                    KeyWords.Add(word);
                                }
                                if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                {
                                    Focus = Focus + " " + word.Value;
                                }
                            }
                        }

                        if(np.Head != null)
                        {
                            foreach (W w in np.Head.W)
                            {
                                Word word = new Word();
                                word.KeyWord = true;
                                word.LEMMA = w.LEMMA;
                                word.offset = w.offset;
                                word.Value = w.Value;
                                word.POS = GetPartOfSpeech(w.POS);
                                word.Score = 75;
                                sentance.Words.Add(word);
                                if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                {
                                    KeyWords.Add(word);
                                }
                                if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                {
                                    Focus = Focus + " " + word.Value;
                                }
                            }
                        }

                        if(np.NPP != null)
                        {
                            foreach (NP npp in np.NPP)
                            {
                                if (npp.W != null)
                                {
                                    if (npp.TYPE == "PERSON")
                                    {
                                        type = QuestionType.FactoidPerson;
                                    }
                                    if (npp.TYPE == "LOCATION")
                                    {
                                        type = QuestionType.FactoidLocation;
                                    }
                                    foreach (W w in npp.W)
                                    {
                                        Word word = new Word();
                                        word.KeyWord = true;
                                        word.LEMMA = w.LEMMA;
                                        word.offset = w.offset;
                                        word.Value = w.Value;
                                        word.POS = GetPartOfSpeech(w.POS);
                                        word.Score = 50;
                                        sentance.Words.Add(word);
                                        if (word.POS == PartOfSpeech.NOUN)
                                        {
                                            KeyWords.Add(word);
                                        }
                                        if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                        {
                                            Focus = Focus + " " + word.Value;
                                        }
                                    }
                                }

                                if (npp.Head != null)
                                {
                                    foreach (W w in npp.Head.W)
                                    {
                                        Word word = new Word();
                                        word.KeyWord = true;
                                        word.LEMMA = w.LEMMA;
                                        word.offset = w.offset;
                                        word.Value = w.Value;
                                        word.POS = GetPartOfSpeech(w.POS);
                                        word.Score = 75;
                                        sentance.Words.Add(word);
                                        if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                        {
                                            KeyWords.Add(word);
                                        }
                                        if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                        {
                                            Focus = Focus + " " + word.Value;
                                        }
                                    }
                                }

                                if (npp.NPP != null)
                                {
                                    foreach (NP nppp in npp.NPP)
                                    {
                                        if (nppp.W != null)
                                        {
                                            if (nppp.TYPE == "PERSON")
                                            {
                                                type = QuestionType.FactoidPerson;
                                            }
                                            if (nppp.TYPE == "LOCATION")
                                            {
                                                type = QuestionType.FactoidLocation;
                                            }

                                            foreach (W w in nppp.W)
                                            {
                                                Word word = new Word();
                                                word.KeyWord = true;
                                                word.LEMMA = w.LEMMA;
                                                word.offset = w.offset;
                                                word.Value = w.Value;
                                                word.POS = GetPartOfSpeech(w.POS);
                                                word.Score = 50;
                                                sentance.Words.Add(word);
                                                if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                                {
                                                    KeyWords.Add(word);
                                                }
                                                if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                                {
                                                    Focus = Focus + " " + word.Value;
                                                }
                                            }
                                        }

                                        if (nppp.Head != null)
                                        {
                                            foreach (W w in nppp.Head.W)
                                            {
                                                Word word = new Word();
                                                word.KeyWord = true;
                                                word.LEMMA = w.LEMMA;
                                                word.offset = w.offset;
                                                word.Value = w.Value;
                                                word.POS = GetPartOfSpeech(w.POS);
                                                word.Score = 75;
                                                sentance.Words.Add(word);
                                                if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                                {
                                                    KeyWords.Add(word);
                                                }
                                                if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                                {
                                                    Focus = Focus + " " + word.Value;
                                                }
                                            }
                                        }

                                        if(nppp.NPP != null)
                                        {
                                            foreach (NP npppp in nppp.NPP)
                                            {
                                                if (npppp.W != null)
                                                {
                                                    if (npppp.TYPE == "PERSON")
                                                    {
                                                        type = QuestionType.FactoidPerson;
                                                    }
                                                    if (npppp.TYPE == "LOCATION")
                                                    {
                                                        type = QuestionType.FactoidLocation;
                                                    }

                                                    foreach (W w in npppp.W)
                                                    {
                                                        Word word = new Word();
                                                        word.KeyWord = true;
                                                        word.LEMMA = w.LEMMA;
                                                        word.offset = w.offset;
                                                        word.Value = w.Value;
                                                        word.POS = GetPartOfSpeech(w.POS);
                                                        word.Score = 50;
                                                        sentance.Words.Add(word);
                                                        if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                                        {
                                                            KeyWords.Add(word);
                                                        }
                                                        if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                                        {
                                                            Focus = Focus + " " + word.Value;
                                                        }
                                                    }
                                                }

                                                if (npppp.Head != null)
                                                {
                                                    foreach (W w in npppp.Head.W)
                                                    {
                                                        Word word = new Word();
                                                        word.KeyWord = true;
                                                        word.LEMMA = w.LEMMA;
                                                        word.offset = w.offset;
                                                        word.Value = w.Value;
                                                        word.POS = GetPartOfSpeech(w.POS);
                                                        word.Score = 75;
                                                        sentance.Words.Add(word);
                                                        if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                                        {
                                                            KeyWords.Add(word);
                                                        }
                                                        if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                                        {
                                                            Focus = Focus + " " + word.Value;
                                                        }
                                                    }
                                                }

                                                if(npppp.NPP != null)
                                                {
                                                    foreach (NP nppppp in npppp.NPP)
                                                    {
                                                        if (nppppp.W != null)
                                                        {
                                                            if (npppp.TYPE == "PERSON")
                                                            {
                                                                type = QuestionType.FactoidPerson;
                                                            }
                                                            if (npppp.TYPE == "LOCATION")
                                                            {
                                                                type = QuestionType.FactoidLocation;
                                                            }

                                                            foreach (W w in nppppp.W)
                                                            {
                                                                Word word = new Word();
                                                                word.KeyWord = true;
                                                                word.LEMMA = w.LEMMA;
                                                                word.offset = w.offset;
                                                                word.Value = w.Value;
                                                                word.POS = GetPartOfSpeech(w.POS);
                                                                word.Score = 50;
                                                                sentance.Words.Add(word);
                                                                if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                                                {
                                                                    KeyWords.Add(word);
                                                                }
                                                                if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                                                {
                                                                    Focus = Focus + " " + word.Value;
                                                                }
                                                            }
                                                        }

                                                        if (nppppp.Head != null)
                                                        {
                                                            foreach (W w in nppppp.Head.W)
                                                            {
                                                                Word word = new Word();
                                                                word.KeyWord = true;
                                                                word.LEMMA = w.LEMMA;
                                                                word.offset = w.offset;
                                                                word.Value = w.Value;
                                                                word.POS = GetPartOfSpeech(w.POS);
                                                                word.Score = 75;
                                                                sentance.Words.Add(word);
                                                                if (word.POS == PartOfSpeech.NOUN || word.POS == PartOfSpeech.ADJECTIVE)
                                                                {
                                                                    KeyWords.Add(word);
                                                                }
                                                                if (char.IsUpper(word.Value[0]) || word.POS == PartOfSpeech.NOUN)
                                                                {
                                                                    Focus = Focus + " " + word.Value;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return sentance;
        }
    }
}
