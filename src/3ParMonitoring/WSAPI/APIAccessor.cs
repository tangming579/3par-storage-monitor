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

        public APIAccessor(string urlWsapi, string user, string password)
        {
            this.urlWsapi = urlWsapi;
            GetSessionKey(user, password);
        }

        public void GetSessionKey(string user, string password)
        {
            if (sessionKey != null) return;
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
            Action<string> callBack = (str) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void StatMemory()
        {
            string url = urlWsapi + "systemreporter/vstime/cachememorystatistics/hires";
            Action<string> callBack = (str) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void QuaryAllPorts()
        {
            string url = urlWsapi + "ports";
            Action<string> callBack = (str) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void QuaryCPGs()
        {
            string url = urlWsapi + "cpgs";
            Action<string> callBack = (str) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
        public void GetSystemInfo()
        {
            string url = urlWsapi + "system";
            Action<string> callBack = (str) =>
            {

            };
            WebClientManager.Get(url, sessionKey, callBack);
        }
    }
}
