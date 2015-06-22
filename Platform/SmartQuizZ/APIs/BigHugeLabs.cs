using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIs
{
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    //use for synonims only for english
    public class BigHugeLabs
    {
        private string _apiKey;
        private string _requestUri;

        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }

        public BigHugeLabs()
        {
            _apiKey = "d4abdec51cc77e73bdae63a7ea52caf2";
            _requestUri = "http://words.bighugelabs.com/api/2/719e8b7de0c4ab0180923e78a152f7c9/";
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "text/json";
            PostData = "";
        }

        public string CallApi(string word)
        {
            _requestUri = _requestUri + word + "/json";
            return  MakeRequest(_requestUri);
        }

        public string MakeRequest(string _requestUri)
        {
            var request = (HttpWebRequest)WebRequest.Create(_requestUri);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;
            var responseValue = string.Empty;
             var asyncResult = request.BeginGetResponse(
                ar =>
                {
                    using (var response = (HttpWebResponse)request.EndGetResponse(ar))
                    {
                                    

                                    if (response.StatusCode != HttpStatusCode.OK)
                                    {
                                        var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                                        throw new ApplicationException(message);
                                    }

              
                                    using (var responseStream = response.GetResponseStream())
                                    {
                                        if (responseStream != null)
                                            using (var reader = new StreamReader(responseStream))
                                            {
                                                responseValue = reader.ReadToEnd();
                                            }
                                    }
                    }
             }, null);

                return responseValue;
            }
        

    }
}
