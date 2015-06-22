using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs
{
    public class Category
    {
        private readonly Alchemy alchemyObj;
        private readonly string Text;
        public string Xml;

        public Category(string text)
        {
            Text = text;
            alchemyObj = new Alchemy();
        }

        public string GetCategory()
        {
            Xml = alchemyObj.TextGetCategory(Text);
            return Xml;
        }
    }
}
