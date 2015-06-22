using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs
{
    public class Keywords
    {
        private Alchemy alchemyObj;
        private string Text;
        public string xml;

        public Keywords(string text)
        {
            Text = text;
            alchemyObj = new Alchemy();
            
        }

        public string GetKeywords()
        {
            if (Text != string.Empty)
            {
                xml = alchemyObj.TextGetRankedKeywords(Text);
                return xml;
            }
            return null;
        }
    }
}
