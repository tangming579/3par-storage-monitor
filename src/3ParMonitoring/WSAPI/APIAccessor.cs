using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ParMonitoring.WSAPI
{
    /*
     * showwsapi -d
     * startwsapi
     * setwsapi -https enable
     * showwsapisession
     */
    public class APIAccessor
    {
        private string urlWsapi;
        private string sessionKey;
        private bool credentialed;
        private DateTime sessionKeyTime;

        public APIAccessor(string urlWsapi, string user, string password)
        {
            this.urlWsapi = urlWsapi;
            sessionKeyTime = DateTime.MinValue;
            GetSessionKey(user, password);
        }

        public void GetSessionKey(string user, string password)
        {
            var totalSecond = (DateTime.Now - sessionKeyTime).TotalSeconds;
            //SessionKey is not repeated within 10s
            if (totalSecond < 10) return;
            sessionKeyTime = DateTime.Now;

            string url = urlWsapi + "credentials";
            var jdata = new JObject();
            jdata["user"] = user;
            jdata["password"] = password;
            jdata["sessionType"] = 1;
            Action<string> callBack = (str) =>
             {
                 JObject obj = JObject.Parse(str);
                 sessionKey = obj.SelectToken("key") + "";
             };
            WebClientManager.Post(url, jdata + "", null, callBack);
        }

        public void StatCPU()
        {
            string url = urlWsapi + "systemreporter/attime/cpustatistics/hires;groupby:node";
            Action<ResponseResult> callBack = (result) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void StatMemory()
        {
            string url = urlWsapi + "systemreporter/vstime/cachememorystatistics/hires";
            Action<ResponseResult> callBack = (result) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void QuaryAllPorts()
        {
            string url = urlWsapi + "ports";
            Action<ResponseResult> callBack = (result) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void QuaryCPGs()
        {
            string url = urlWsapi + "cpgs";
            Action<ResponseResult> callBack = (result) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void GetSystemInfo()
        {
            string url = urlWsapi + "system";
            Action<ResponseResult> callBack = (result) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
    }
}
