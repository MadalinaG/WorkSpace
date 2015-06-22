using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using SmartQA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SmartQA.TextExtraction
{
    public class PdfProcessing
    {
        private readonly string pathToPdfDocument;
        private StringBuilder documentData;
        private List<BGDocument> BackgroundDocuments;
        private readonly string URL;
        public List<TextDocument> TextFromDocuments;
        private int ReadFromPage = 0;
        private int StopAtPage = 0;

        public PdfProcessing(List<BGDocument> BackgroundDocuments)
        {

            this.BackgroundDocuments = BackgroundDocuments;

        }

        public PdfProcessing(string url, int readFromPage, int stopAtPage)
        {
            URL = url;
            ReadFromPage = readFromPage;
            StopAtPage = stopAtPage;
        }

        private string ExtractText(string documentPath)
        {
            string stringToReturn = string.Empty;
            documentData = new StringBuilder();
            try
            {
                using (var reader = new PdfReader(documentPath))
                {
                    if (ReadFromPage > reader.NumberOfPages || ReadFromPage == 0)
                    {
                        ReadFromPage = 1;
                    }

                    if (StopAtPage > reader.NumberOfPages || StopAtPage == 0)
                    {
                        StopAtPage = reader.NumberOfPages;
                    }

                    for (var i = ReadFromPage; i <= StopAtPage; i++)
                    {
                        documentData.Append(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));
                    }
                    if (documentData.Length > 0)
                    {
                        stringToReturn = documentData.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //scrie in logouri eroarea cel mai probabil in baza de date
            }

            return stringToReturn;
        }

        public List<TextDocument> GetAllText()
        {

            TextFromDocuments = new List<TextDocument>();

            if (!string.IsNullOrEmpty(URL))   //se proceseaza un test
            {
                TextFromDocuments.Add(CreateTextDocument(URL, "", ExtractText(URL)));
            }
            else
            {
                if (BackgroundDocuments.Count() > 0)
                {
                    foreach (BGDocument bd in BackgroundDocuments)
                    {
                        TextFromDocuments.Add(CreateTextDocument(bd.Path, bd.Title, ExtractText(bd.Path)));

                    }

                }
            }
            return TextFromDocuments;
        }

        private static TextDocument CreateTextDocument(string path, string documentName, string text)
        {

            var document = new TextDocument { Path = path, DocumentName = documentName, Text = text };
            return document;
        }
    }
}