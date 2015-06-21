using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs
{
    public class Concept
    {
        private readonly Alchemy alchemyObj;
        private readonly string Text;
        public string Xml;

        public Concept(string text)
        {
            Text = text;
            alchemyObj = new Alchemy();
        }

        public string GetConcept()
        {
            Xml = alchemyObj.URLGetRankedConcepts(Text);
            return Xml;
        }
    }
}
