using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                    result = Encoding.UTF8.GetString(e.Result);
                    callback?.Invoke(result);
                }
                catch (Exception exp)
                {
                    result = "Error:" + (exp.InnerException?.Message ?? exp.Message);
                }
            };
            client.UploadDataAsync(new Uri(url), "POST", postData);
        }

        static ParWebClient CreateWebClient(string sessionKey)
        {
            ParWebClient webClient = new ParWebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
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
