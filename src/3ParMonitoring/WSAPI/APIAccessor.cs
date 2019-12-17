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
        private string sessionKey = null;
        private bool credentialed;

        public APIAccessor(string urlWsapi, string user, string password)
        {
            this.urlWsapi = urlWsapi;
            GetSessionKey(user, password);
        }

        public void GetSessionKey(string user, string password)
        {
            string url = urlWsapi + "credentials";
            var jdata = new JObject();
            jdata["user"] = user;
            jdata["password"] = password;
            jdata["sessionType"] = 1;
            Action<string> callBack = (str) =>
             {

             };
            WebClientManager.Post(url, jdata + "", null, callBack);
        }

        public void StatCPU()
        {
            string url = urlWsapi + "credentials";
            Action<string> callBack = (str) =>
            {

            };
            WebClientManager.Post(url, url + "", sessionKey, callBack);
        }
    }
}
