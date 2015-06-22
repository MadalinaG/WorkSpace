using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs
{
    public class Relations
    {
        private readonly Alchemy alchemyObj;
        private readonly string Text;
        public string Xml;

        public Relations(string text)
        {
            Text = text;
            alchemyObj = new Alchemy();
        }

        public string GetRelations()
        {
            Xml = alchemyObj.TextGetRelations(Text);
            return Xml;
        }
    }
}
