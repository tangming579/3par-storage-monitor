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

        public static void Get(string url, string sessionKey, Action<ResponseResult> callback)
        {
            var client = CreateWebClient(sessionKey);
            client.DownloadStringCompleted += (sender, e) =>
            {
                string result = string.Empty;
                try
                {
                    if (e.Error != null)
                    {
                        if (e.Error is WebException)
                        {
                            WebException we = (WebException)e.Error;
                            using (HttpWebResponse hr = (HttpWebResponse)we.Response)
                            {
                                int statusCode = (int)hr.StatusCode;
                                using (StreamReader sr = new StreamReader(hr.GetResponseStream(), Encoding.UTF8))
                                {
                                    var resResult = new ResponseResult() { IsSuccess = false };
                                    resResult.Result = JObject.Parse(sr.ReadToEnd());
                                    resResult.StatusCode = statusCode;
                                    callback?.Invoke(resResult);
                                }
                            };
                        }
                        else
                        {
                            var resResult = new ResponseResult() { IsSuccess = false };
                            resResult.StatusCode = (int)client.StatusCode();
                            callback?.Invoke(resResult);
                        }
                    }
                    else
                    {
                        var resResult = new ResponseResult() { IsSuccess = true };
                        resResult.Result = JObject.Parse(e.Result);
                        resResult.StatusCode = 200;
                        callback?.Invoke(resResult);
                    }

                }
                catch (Exception exp)
                {
                    result = "Error:" + (exp.InnerException?.Message ?? exp.Message);
                    var resResult = new ResponseResult() { IsSuccess = false };
                    resResult.Result = new JObject();
                    resResult.Message = result;
                    resResult.StatusCode = (int)client.StatusCode();
                    callback?.Invoke(resResult);
                }
            };
            client.DownloadStringAsync(new Uri(url));
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
        private WebRequest request = null;

        public ParWebClient()
        {
            this.timeOut = int.Parse(ConfigurationManager.AppSettings["webClientTimeOut"]);
            this.Encoding = Encoding.UTF8;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            request = base.GetWebRequest(address);
            request.Timeout = timeOut;
            if (this.request is HttpWebRequest)
            {
                ((HttpWebRequest)request).ReadWriteTimeout = timeOut;
                ((HttpWebRequest)request).AllowAutoRedirect = false;
            }
            return request;
        }

        public HttpStatusCode StatusCode()
        {
            HttpStatusCode result;
            try
            {
                if (this.request == null)
                {
                    return HttpStatusCode.Forbidden;
                }
                HttpWebResponse response = base.GetWebResponse(this.request) as HttpWebResponse;
                if (response != null)
                {
                    result = response.StatusCode;
                }
                else
                {
                    throw (new InvalidOperationException("Unable to retrieve the status " +
                        "code, maybe you haven't made a request yet."));
                }
            }
            catch (Exception exp)
            {
                return HttpStatusCode.Forbidden;
            }
            return result;
        }
    }

    public class ResponseResult
    {
        public int StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public JObject Result { get; set; }
    }
}
