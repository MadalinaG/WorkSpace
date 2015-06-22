using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs
{
    public class Entity
    {

        private readonly Alchemy alchemyObj;
        private readonly string Text;
        public string Xml;

        public Entity(string text)
        {
            Text = text;
            alchemyObj = new Alchemy();
        }

        public string GetEntity()
        {
            Xml = alchemyObj.TextGetRankedNamedEntities(Text);
            return Xml;
        }
    }
}
