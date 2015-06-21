using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexerLucene
{
    public interface ILuceneService
    {
        void BuildIndex(IEnumerable<BackgroundDocument> dataToIndex);
        List<BackgroundDocument> Search(string searchTerm, string searchField);
    }
}
