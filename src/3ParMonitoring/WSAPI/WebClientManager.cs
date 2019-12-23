using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _3ParMonitoring
{
    public static class WebClientManager
    {
        public static void Post(string url, string jsonStr, string sessionKey, Action<string> callback)
        {
            var client = CreateWebClient(sessionKey);
            byte[] postData = Encoding.UTF8.GetBytes(jsonStr);
            client.UploadDataCompleted += (sender, e) =>
            {
                string result = string.Empty;
                try
                {
                    if (e.Error != null && e.Error is WebException)
                    {
                        WebException we = (WebException)e.Error;
                        using (HttpWebResponse hr = (HttpWebResponse)we.Response)
                        {
                            int statusCode = (int)hr.StatusCode;
                            StringBuilder sb = new StringBuilder();
                            StreamReader sr = new StreamReader(hr.GetResponseStream(), Encoding.UTF8);
                            sb.Append(sr.ReadToEnd());
                            callback?.Invoke(sb.ToString());
                        }
                    }
                    else
                    {
                        result = Encoding.UTF8.GetString(e.Result);
                        callback?.Invoke(result);
                    }

                }
                catch (WebException ex)
                {
                    if (ex.GetType().Name == "WebException")
                    {

                    }
                }
                catch (Exception exp)
                {
                    result = "Error:" + (exp.InnerException?.Message ?? exp.Message);
                }
            };
            client.UploadDataAsync(new Uri(url), "POST", postData);
        }

        public static void Get(string url, string sessionKey, Action<string> callback)
        {
            var client = CreateWebClient(sessionKey);
            client.DownloadDataCompleted += (sender, e) =>
            {
                string result = string.Empty;
                try
                {
                    if (e.Error != null && e.Error is WebException)
                    {
                        WebException we = (WebException)e.Error;
                        using (HttpWebResponse hr = (HttpWebResponse)we.Response)
                        {
                            int statusCode = (int)hr.StatusCode;
                            StringBuilder sb = new StringBuilder();
                            StreamReader sr = new StreamReader(hr.GetResponseStream(), Encoding.UTF8);
                            sb.Append(sr.ReadToEnd());
                            callback?.Invoke(sb.ToString());
                        }
                    }
                    else
                    {
                        result = Encoding.UTF8.GetString(e.Result);
                        callback?.Invoke(result);
                    }

                }
                catch (WebException ex)
                {
                    if (ex.GetType().Name == "WebException")
                    {

                    }
                }
                catch (Exception exp)
                {
                    result = "Error:" + (exp.InnerException?.Message ?? exp.Message);
                }
            };
            client.DownloadDataAsync(new Uri(url));
        }

        static ParWebClient CreateWebClient(string sessionKey)
        {
            ParWebClient webClient = new ParWebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            if (!string.IsNullOrEmpty(sessionKey))
                webClient.Headers.Add("X-HP3PAR-WSAPI-SessionKey", sessionKey);

            return webClient;
        }
    }

    public class ParWebClient : WebClient
    {
        private int timeOut;
        public ParWebClient()
        {
            this.timeOut = int.Parse(ConfigurationManager.AppSettings["webClientTimeOut"]);
            this.Encoding = Encoding.UTF8;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            return request;
        }
    }
}
