using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ParMonitoring.WSAPI
{
    /*
     * showwsapi
     * startwsapi
     * setwsapi -https enable
     * showwsapisession
     */
    public class APIAccessor
    {
        private string urlWsapi;
        private string sessionKey = null;
        private bool credentialed;
        private const string SessionHeader = "X-HP3PAR-WSAPI-SessionKey";

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
        }
    }
}
