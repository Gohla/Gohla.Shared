using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Gohla.Shared
{
    public class HttpWebRequestHelper
    {
        private Func<String, String> _urlModifier;

        /**
        Default constructor.
        **/
        public HttpWebRequestHelper()
            : this(str => str)
        {

        }

        /**
        Constructor with URL modifier.
        
        @param  urlModifier The URL modifier that gets invoked on all URLs, can be used to transform URLs before
                            creating HTTP requests.
        **/
        public HttpWebRequestHelper(Func<String, String> urlModifier)
        {
            _urlModifier = urlModifier;
        }

        /**
        Creates a HttpWebRequest object for given url.
        
        @param  url         URL of the document.
        @param  method      (optional) The HTTP method, defaults to GET.
        @param  keepAlive   (optional) The HTTP connection keep alive flag, defaults to true.
        @param  proxy       (optional) The proxy object, defaults to null (no proxy, fast).
        
        @return HttpWebRequest object for given url.
        **/
        public HttpWebRequest CreateRequest(String url, String method = WebRequestMethods.Http.Get,
            bool keepAlive = true, IWebProxy proxy = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_urlModifier.Invoke(url));
            request.Method = method;
            request.KeepAlive = keepAlive;
            request.Proxy = proxy;

            return request;
        }

        /**
        Gets the HTTP response as a string.
        
        @param  response    The HTTP response.
        
        @return HTTP response as a string.
        **/
        public String ResponseString(HttpWebResponse response)
        {
            byte[] buffer = new byte[8192];
            StringBuilder stringBuilder = new StringBuilder();

            Stream responseStream = response.GetResponseStream();
            int count = 0;

            do
            {
                // Fill the buffer with data.
                count = responseStream.Read(buffer, 0, buffer.Length);

                // Make sure we read some data.
                if(count != 0)
                {
                    // Translate from bytes to ASCII text and add to string builder.
                    stringBuilder.Append(Encoding.ASCII.GetString(buffer, 0, count));
                }
            }
            while(count > 0); // Any more data to read?

            responseStream.Close();

            return stringBuilder.ToString();
        }

        /**
        Creates an HTTP request and gets the response as a string.
        
        @param  url URL of the document.
        
        @return HTTP response as a string.
        **/
        public String CreateRequestString(String url)
        {
            HttpWebRequest request = CreateRequest(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            String responseString = ResponseString(response);
            response.Close();
            return responseString;
        }

        /**
        Gets the HTTP response as an XDocument.
        
        @param  response    The HTTP response.
        
        @return HTTP response as an XDocument.
        **/
        public XDocument ResponseXML(HttpWebResponse response)
        {
            Stream responseStream = response.GetResponseStream();
            XDocument xDoc = XDocument.Load(responseStream);
            responseStream.Close();
            return xDoc;
        }

        /**
        Creates an HTTP request and gets the response as an XDocument.
        
        @param  url URL of the document.
        
        @return HTTP response as an XDocument.
        **/
        public XDocument CreateRequestXML(String url)
        {
            HttpWebRequest request = CreateRequest(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XDocument xDoc = ResponseXML(response);
            response.Close();
            return xDoc;
        }

        /**
        Creates an HTTP request and ignores the response.
        
        @param  url URL of the document.
        **/
        public void CreateRequestIgnore(String url)
        {
            HttpWebRequest request = CreateRequest(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            responseStream.Close();
            response.Close();
        }
    }
}
