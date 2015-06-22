using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace APIs
{
    public class Alchemy
    {
        private string _apiKey;
        private string _requestUri;

        public Alchemy()
        {
            _apiKey = "682e4e71d12687f9d34f061ef32e200d4076053a";
            _requestUri = "http://access.alchemyapi.com/calls/";
        }

        public void SetAPIHost(string apiHost)
        {
            if (apiHost.Length < 2)
            {
                var ex =
                    new System.ApplicationException("Error setting API host.");

                throw ex;
            }

            _requestUri = "http://" + apiHost + ".alchemyapi.com/calls/";
        }

        public void SetAPIKey(string apiKey)
        {
            _apiKey = apiKey;

            if (_apiKey.Length < 5)
            {
                var ex =
                    new System.ApplicationException("Error setting API key.");

                throw ex;
            }
        }

      

        #region GetRankedKeywords
        public string URLGetRankedKeywords(string url)
        {
            CheckURL(url);

            return URLGetRankedKeywords(url, new AlchemyAPI_KeywordParams());
        }

        public string URLGetRankedKeywords(string url, AlchemyAPI_KeywordParams parameters)
        {
            CheckURL(url);
            parameters.setUrl(url);

            return GET("URLGetRankedKeywords", "url", parameters);
        }

        public string HTMLGetRankedKeywords(string html, string url)
        {
            CheckHTML(html, url);

            return HTMLGetRankedKeywords(html, url, new AlchemyAPI_KeywordParams());
        }

        public string HTMLGetRankedKeywords(string html, string url, AlchemyAPI_KeywordParams parameters)
        {
            CheckHTML(html, url);
            parameters.setHtml(html);
            parameters.setUrl(url);

            return POST("HTMLGetRankedKeywords", "html", parameters);
        }

        public string TextGetRankedKeywords(string text)
        {
            CheckText(text);

            return TextGetRankedKeywords(text, new AlchemyAPI_KeywordParams());
        }

        public string TextGetRankedKeywords(string text, AlchemyAPI_KeywordParams parameters)
        {
            CheckText(text);
            parameters.setText(text);

            return POST("TextGetRankedKeywords", "text", parameters);
        }
        #endregion


        #region GetLanguage
        public string URLGetLanguage(string url)
        {
            CheckURL(url);

            return URLGetLanguage(url, new AlchemyAPI_LanguageParams());
        }

        public string URLGetLanguage(string url, AlchemyAPI_LanguageParams parameters)
        {
            CheckURL(url);
            parameters.setUrl(url);

            return GET("URLGetLanguage", "url", parameters);
        }

        public string HTMLGetLanguage(string html, string url)
        {
            CheckHTML(html, url);

            return HTMLGetLanguage(html, url, new AlchemyAPI_LanguageParams());
        }

        public string HTMLGetLanguage(string html, string url, AlchemyAPI_LanguageParams parameters)
        {
            CheckHTML(html, url);
            parameters.setHtml(html);
            parameters.setUrl(url);

            return POST("HTMLGetLanguage", "html", parameters);
        }

        public string TextGetLanguage(string text)
        {
            CheckText(text);

            return TextGetLanguage(text, new AlchemyAPI_LanguageParams());
        }

        public string TextGetLanguage(string text, AlchemyAPI_LanguageParams parameters)
        {
            CheckText(text);
            parameters.setText(text);

            return POST("TextGetLanguage", "text", parameters);
        }
        #endregion

        private void CheckHTML(string html, string url)
        {
            if (html.Length < 10)
            {
                var ex =
                new System.ApplicationException("Enter a HTML document to analyze.");

                throw ex;
            }

            if (url.Length < 10)
            {
                var ex =
                new System.ApplicationException("Enter a web URL to analyze.");

                throw ex;
            }
        }

        private void CheckText(string text)
        {
            if (text.Length < 5)
            {
                var ex =
                new System.ApplicationException("Enter some text to analyze.");

                throw ex;
            }
        }

        private void CheckURL(string url)
        {
            if (url.Length < 10)
            {
                var ex =
                new System.ApplicationException("Enter a web URL to analyze.");

                throw ex;
            }
        }

        private string GET(string callName, string callPrefix, AlchemyAPI_BaseParams parameters)
        { // callMethod, callPrefix, ... params
            var uri = new StringBuilder();
            uri.Append(_requestUri).Append(callPrefix).Append("/").Append(callName);
            uri.Append("?apikey=").Append(_apiKey).Append(parameters.getParameterString());

            parameters.resetBaseParams();

            var address = new Uri(uri.ToString());
            var wreq = WebRequest.Create(address) as HttpWebRequest;
            wreq.Proxy = null;

            byte[] postData = parameters.GetPostData();

            if (postData == null)
            {
                wreq.Method = "GET";
            }
            else
            {
                wreq.Method = "POST";
                using (var ps = wreq.GetRequestStream())
                {
                    ps.Write(postData, 0, postData.Length);
                }
            }

            return DoRequest(wreq, parameters.getOutputMode());
        }

        private string POST(string callName, string callPrefix, AlchemyAPI_BaseParams parameters)
        { // callMethod, callPrefix, ... params
            Uri address = new Uri(_requestUri + callPrefix + "/" + callName);

            var wreq = WebRequest.Create(address) as HttpWebRequest;
            wreq.Proxy = null;
            wreq.Method = "POST";
            wreq.ContentType = "application/x-www-form-urlencoded";

            var d = new StringBuilder();
            d.Append("apikey=").Append(_apiKey).Append(parameters.getParameterString());

            parameters.resetBaseParams();

            byte[] bd = Encoding.UTF8.GetBytes(d.ToString());

            wreq.ContentLength = bd.Length;
            using (Stream ps = wreq.GetRequestStream())
            {
                ps.Write(bd, 0, bd.Length);
            }

            return DoRequest(wreq, parameters.getOutputMode());
        }

        private string DoRequest(HttpWebRequest wreq, AlchemyAPI_BaseParams.OutputMode outputMode)
        {
            using (var wres = wreq.GetResponse() as HttpWebResponse)
            {
                var r = new StreamReader(wres.GetResponseStream());

                string xml = r.ReadToEnd();

                if (string.IsNullOrEmpty(xml))
                    throw new XmlException("The API request returned back an empty response. Please verify that the url is correct.");

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var root = xmlDoc.DocumentElement;

                if (AlchemyAPI_BaseParams.OutputMode.XML == outputMode)
                {
                    if (root != null)
                    {
                        XmlNode status = root.SelectSingleNode("/results/status");

                        if (status.InnerText != "OK")
                        {
                            string errorMessage = "Error making API call.";

                            try
                            {
                                XmlNode statusInfo = root.SelectSingleNode("/results/statusInfo");
                                if (statusInfo != null) errorMessage = statusInfo.InnerText;
                            }
                            catch
                            {
                                errorMessage = "An error occurred: Unable to access XmlNode /results/statusInfo";
                            }
                            var ex = new System.ApplicationException(errorMessage);

                            throw ex;
                        }
                    }
                }
                else if (AlchemyAPI_BaseParams.OutputMode.RDF == outputMode)
                {
                    var nm = new XmlNamespaceManager(xmlDoc.NameTable);
                    nm.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                    nm.AddNamespace("aapi", "http://rdf.alchemyapi.com/rdf/v1/s/aapi-schema#");
                    XmlNode status = root.SelectSingleNode("/rdf:RDF/rdf:Description/aapi:ResultStatus", nm);

                    if (status.InnerText != "OK")
                    {
                        string errorMessage = "Error making API call.";

                        try
                        {
                            XmlNode statusInfo = root.SelectSingleNode("/results/statusInfo");
                            errorMessage = statusInfo.InnerText;
                        }
                        catch
                        {
                            errorMessage = "An error occurred: Unable to access XmlNode /results/statusInfo";
                        }
                        var ex = new System.ApplicationException(errorMessage);

                        throw ex;
                    }
                }

                return xml;

            }
        }
        #region GetCategory
        public string URLGetCategory(string url)
        {
            CheckURL(url);

            return URLGetCategory(url, new AlchemyAPI_CategoryParams());
        }

        public string URLGetCategory(string url, AlchemyAPI_CategoryParams parameters)
        {
            CheckURL(url);
            parameters.setUrl(url);

            return GET("URLGetCategory", "url", parameters);
        }

        public string HTMLGetCategory(string html, string url)
        {
            CheckHTML(html, url);

            return HTMLGetCategory(html, url, new AlchemyAPI_CategoryParams());
        }

        public string HTMLGetCategory(string html, string url, AlchemyAPI_CategoryParams parameters)
        {
            CheckHTML(html, url);
            parameters.setHtml(html);
            parameters.setUrl(url);

            return POST("HTMLGetCategory", "html", parameters);
        }

        public string TextGetCategory(string text)
        {
            CheckText(text);

            return TextGetCategory(text, new AlchemyAPI_CategoryParams());
        }

        public string TextGetCategory(string text, AlchemyAPI_CategoryParams parameters)
        {
            CheckText(text);
            parameters.setText(text);

            return POST("TextGetCategory", "text", parameters);
        }
        #endregion
        #region GetRelations
        public string URLGetRelations(string url)
        {
            CheckURL(url);

            return URLGetRelations(url, new AlchemyAPI_RelationParams());
        }

        public string URLGetRelations(string url, AlchemyAPI_RelationParams parameters)
        {
            CheckURL(url);
            parameters.setUrl(url);

            return GET("URLGetRelations", "url", parameters);
        }

        public string HTMLGetRelations(string html, string url)
        {
            CheckHTML(html, url);

            return HTMLGetRelations(html, url, new AlchemyAPI_RelationParams());
        }

        public string HTMLGetRelations(string html, string url, AlchemyAPI_RelationParams parameters)
        {
            CheckHTML(html, url);
            parameters.setHtml(html);
            parameters.setUrl(url);

            return POST("HTMLGetRelations", "html", parameters);
        }

        public string TextGetRelations(string text)
        {
            CheckText(text);

            return TextGetRelations(text, new AlchemyAPI_RelationParams());
        }

        public string TextGetRelations(string text, AlchemyAPI_RelationParams parameters)
        {
            CheckText(text);
            parameters.setText(text);

            return POST("TextGetRelations", "text", parameters);
        }
        #endregion
        #region GetRankedConcepts
        public string URLGetRankedConcepts(string url)
        {
            CheckURL(url);

            return URLGetRankedConcepts(url, new AlchemyAPI_ConceptParams());
        }

        public string URLGetRankedConcepts(string url, AlchemyAPI_ConceptParams parameters)
        {
            CheckURL(url);
            parameters.setUrl(url);

            return GET("URLGetRankedConcepts", "url", parameters);
        }

        public string HTMLGetRankedConcepts(string html, string url)
        {
            CheckHTML(html, url);

            return HTMLGetRankedConcepts(html, url, new AlchemyAPI_ConceptParams());
        }

        public string HTMLGetRankedConcepts(string html, string url, AlchemyAPI_ConceptParams parameters)
        {
            CheckHTML(html, url);
            parameters.setHtml(html);
            parameters.setUrl(url);

            return POST("HTMLGetRankedConcepts", "html", parameters);
        }

        public string TextGetRankedConcepts(string text)
        {
            CheckText(text);

            return TextGetRankedConcepts(text, new AlchemyAPI_ConceptParams());
        }

        public string TextGetRankedConcepts(string text, AlchemyAPI_ConceptParams parameters)
        {
            CheckText(text);
            parameters.setText(text);

            return POST("TextGetRankedConcepts", "text", parameters);
        }
        #endregion
        #region GetRankedNamedEntities
        public string URLGetRankedNamedEntities(string url)
        {
            CheckURL(url);

            return URLGetRankedNamedEntities(url, new AlchemyAPI_EntityParams());
        }

        public string URLGetRankedNamedEntities(string url, AlchemyAPI_EntityParams parameters)
        {
            CheckURL(url);
            parameters.setUrl(url);

            return GET("URLGetRankedNamedEntities", "url", parameters);
        }

        public string HTMLGetRankedNamedEntities(string html, string url)
        {
            CheckHTML(html, url);

            return HTMLGetRankedNamedEntities(html, url, new AlchemyAPI_EntityParams());
        }


        public string HTMLGetRankedNamedEntities(string html, string url, AlchemyAPI_EntityParams parameters)
        {
            CheckHTML(html, url);
            parameters.setHtml(html);
            parameters.setUrl(url);

            return POST("HTMLGetRankedNamedEntities", "html", parameters);
        }

        public string TextGetRankedNamedEntities(string text)
        {
            CheckText(text);

            return TextGetRankedNamedEntities(text, new AlchemyAPI_EntityParams());
        }

        public string TextGetRankedNamedEntities(string text, AlchemyAPI_EntityParams parameters)
        {
            CheckText(text);
            parameters.setText(text);

            return POST("TextGetRankedNamedEntities", "text", parameters);
        }
        #endregion
       
    }
}
