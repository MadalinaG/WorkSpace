using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IndexerLucene
{
    public class LuceneService : ILuceneService
    {
        //public Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        public Analyzer analyzer = new WhitespaceAnalyzer();

        private static FSDirectory _directoryTemp;
        private IndexWriter writer;
        private string indexPath = @"D:\temp\LuceneIndex";
        private IndexReader reader;
        static readonly char[] whiteSpaces = new char[] { ' ', '\t', '\n', '\r' };
        private FSDirectory directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(indexPath));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(indexPath, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        public LuceneService()
        {
            InitialiseLucene();
        }

        private void InitialiseLucene()
        {
            if (System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.Delete(indexPath, true);
            }

            System.IO.Directory.CreateDirectory(indexPath);
            writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }


        public void BuildIndex(IEnumerable<BackgroundDocument> dataToIndex)
        {
            foreach (var sampleDataFileRow in dataToIndex)
            {
                int i = -1;
                foreach (string text in sampleDataFileRow.Paragraphs)
                {
                    Document doc = new Document();
                    doc.Add(new Field("TestId", sampleDataFileRow.TestID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("Name", sampleDataFileRow.FileName.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                    doc.Add(new Field("Text", text, Field.Store.YES, Field.Index.ANALYZED));
                    //doc.Add(new Field("ProcessedText", sampleDataFileRow.ProcessedParagraphs[++i], Field.Store.YES, Field.Index.ANALYZED));
                    writer.AddDocument(doc);
                }

            }
            writer.Optimize();
            writer.Close();
        }

        public void ClearLuceneIndexRecord(BackgroundDocument document)
        {
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("TestId", document.TestID.ToString()));
                writer.DeleteDocuments(searchQuery);
                
                writer.Dispose();
            }

        }

        private Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private FuzzyQuery fparseQuery(string searchQuery)
        {
            var query = new FuzzyQuery(new Term("Text", searchQuery), 0.5f);
            return query;
        }

        public List<BackgroundDocument> Search(string searchQuery, string searchField = "")
        {
            //if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<BackgroundDocument>();

            //analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            reader = IndexReader.Open(directory, true);

            IndexSearcher searcher = new IndexSearcher(reader);
            var hits_limit = 10;

            IndexWriter.Unlock(directory);
            if (!string.IsNullOrEmpty(searchField))
            {

                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Text", analyzer);
                parser.AllowLeadingWildcard = true;
                //Query query = parseQuery(QueryParser.Escape(searchQuery), parser);
                searchQuery = QueryParser.Escape(searchQuery.Trim());
                Query query = parser.Parse(GetSearchTerm(searchQuery, true));

                var hits = searcher.Search(query, hits_limit).ScoreDocs;
                var results = MapLuceneToDataList(hits, searcher);
                var res = results.ToList();
                for (int i = 0; i < res.Count; i++)
                {
                    res[i].Score = hits.ToList()[i].Score;
                }
                analyzer.Close();
                searcher.Dispose();
                return res;
            }
            else // search by multiple fields (ordered by RELEVANCE)
            {
                var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] { "TestId", "Name", "Text" }, analyzer);
                Query query = parseQuery(searchQuery, parser);
                var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                var results = MapLuceneToDataList(hits, searcher);
                var res = results.ToList();
                for (int i = 0; i < res.Count; i++)
                {
                    res[i].Score = hits.ToList()[i].Score;
                }
                analyzer.Close();
                searcher.Dispose();
                return res;
            }

        }

        string GetSearchTerm(string term, bool allowLeadingWildcard)
        {
            string[] splitted = term.Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries);
            splitted = splitted.Select(item => allowLeadingWildcard ? "*" + item + "*" : item + "*").ToArray();
            return string.Join(" ", splitted);
        }

        private IEnumerable<BackgroundDocument> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }
        private IEnumerable<BackgroundDocument> MapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private BackgroundDocument MapLuceneDocumentToData(Document document)
        {
            return new BackgroundDocument
            {
                TestID = Convert.ToInt32(document.Get("TestId")),
                FileName = document.Get("Name"),
                Paragraph = document.Get("Text"),
                
            };
        }


    }
}
