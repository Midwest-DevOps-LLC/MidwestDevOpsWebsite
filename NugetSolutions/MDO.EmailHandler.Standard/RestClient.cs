using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MDO.EmailHandler.Standard
{
    public class RestClient
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string BaseUrl { get; set; }
        public string Path { get; set; }
        public string FullPath { get { return BaseUrl + Path; } }

        public RestClient(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        public RestClient()
        {

        }

        internal RestClient SetPath(string path)
        {
            this.Path = path;
            return this;
        }

        internal HttpStatusCode GetStatus()
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

        internal string GetRequest()
        {
            return GetRequest(null);
        }

        internal string GetRequest(Dictionary<string, string> parms)
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

        private string GenerateQueryParms(Dictionary<string, string> parms)
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

        internal string PostRequest(string json)
        {
            string result = "";

            var request = (HttpWebRequest)WebRequest.Create(this.FullPath);
            request.ContentType = "application/json";
            request.Method = "POST";

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

        internal string PutRequest(string json)
        {
            string result = "";

            var request = (HttpWebRequest)WebRequest.Create(this.FullPath);
            request.ContentType = "application/json";
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

        internal void DownloadFile(string path)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Other");

                client.DownloadFile(this.FullPath, path);
            }
        }
    }
}
