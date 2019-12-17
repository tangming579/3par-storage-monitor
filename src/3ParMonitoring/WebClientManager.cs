using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _3ParMonitoring
{
    public class WebClientManager
    {
        public void Post()
        {

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
