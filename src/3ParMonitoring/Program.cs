using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _3ParMonitoring
{
    class Program
    {


        static void Main(string[] args)
        {
            // 解决WebClient不能通过https下载内容问题
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                 System.Security.Cryptography.X509Certificates.X509Chain chain,
                 System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true; // **** Always accept
                };

            using (WebClient client = new WebClient())
            {
                string address = "https://192.168.128.151:8080/api/v1/";
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/json; charset=utf-8");
                client.DownloadStringCompleted += Client_DownloadStringCompleted;
                client.DownloadStringAsync(new Uri(address));
            }

            Console.ReadKey();
        }

        private static void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            else
            {
                string a = e.Result;
            }
        }
    }
}
