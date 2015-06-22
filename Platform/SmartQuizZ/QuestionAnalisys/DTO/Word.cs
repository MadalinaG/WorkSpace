using System;
using System.Xml.Serialization;

namespace QuestionAnalisys
{
    [Serializable()]
    public class Word
    {
        public string Value { get; set; }
        public string LEMMA { get; set; }
        public PartOfSpeech POS { get; set; }
        public bool KeyWord { get; set;  }
        public int offset { get; set; }
        public int Score { get; set; }
    }
}
