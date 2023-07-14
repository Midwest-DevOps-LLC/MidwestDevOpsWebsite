using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace MDO.RESTServiceRequestor.Standard
{
    public class RestClient
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string BaseUrl { get; set; }
        public string Path { get; set; }
        public string FullPath { get { return BaseUrl + Path; } }
        public string OverrideContentType { get; set; }

        public RestClient (string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        public RestClient()
        {

        }

        public RestClient SetPath(string path)
        {
            this.Path = path;
            return this;
        }

        public HttpStatusCode GetStatus()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.FullPath);

            if (this.Headers != null)
            {
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                return response.StatusCode;
        }

        public string GetRequest()
        {
            return GetRequest(null);
        }

        public string GetRequest(Dictionary<string, string> parms)
        {
            string html = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.FullPath + GenerateQueryParms(parms));
            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (this.Headers != null)
            {
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }

        public string GenerateQueryParms(Dictionary<string, string> parms)
        {
            var ret = "";

            if (parms != null && parms.Count > 0)
            {
                ret += "?";

                foreach (var parm in parms)
                {
                    ret += parm.Key + "=" + parm.Value + "&";
                }

                ret = ret.Remove(ret.Length - 1, 1);
            }

            return ret;
        }

        public string PostRequest(string json, bool urlEncode = false)
        {
            string result = "";

            var request = (HttpWebRequest)WebRequest.Create(this.FullPath);
            request.ContentType = (string.IsNullOrEmpty(OverrideContentType) ? "application/json" : OverrideContentType);
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                if (urlEncode)
                {
                    streamWriter.Write(HttpUtility.UrlEncode(json));
                }
                else
                {
                    streamWriter.Write(json);
                }
            }

            if (this.Headers != null)
            {
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        public string PutRequest(string json)
        {
            string result = "";

            var request = (HttpWebRequest)WebRequest.Create(this.FullPath);
            request.ContentType = (string.IsNullOrEmpty(OverrideContentType) ? "application/json" : OverrideContentType);
            request.Method = "PUT";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            if (this.Headers != null)
            {
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        public string DeleteRequest(string json)
        {
            string result = "";

            var request = (HttpWebRequest)WebRequest.Create(this.FullPath);
            request.ContentType = (string.IsNullOrEmpty(OverrideContentType) ? "application/json" : OverrideContentType);
            request.Method = "DELETE";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            if (this.Headers != null)
            {
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        public void DownloadFile(string path)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Other");

                client.DownloadFile(this.FullPath, path);
            }
        }
    }
}
