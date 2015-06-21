using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionAnalisys
{
    [Serializable()]
    public  class Sentance
    {
        public List<Word> Words { get; set; }
    }
}
