using _3ParMonitoring.WSAPI;
using Renci.SshNet;
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
            //ServicePointManager.ServerCertificateValidationCallback +=
            //    delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //     System.Security.Cryptography.X509Certificates.X509Chain chain,
            //     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //    {
            //        return true; // **** Always accept
            //    };

            SshClientManager sshClientManager = new SshClientManager("192.168.128.1", "user", "password");
            if (sshClientManager.Connect())
                Console.WriteLine("Ssh connect success!");
            sshClientManager.SrStatCPU();
            sshClientManager.StatMemory();

            APIAccessor aPIAccessor = new APIAccessor("https://192.168.128.1:8080/aip/v1/", "user", "password");
            aPIAccessor.StatCPU();
            aPIAccessor.GetSystemInfo();

            Console.ReadKey();
            sshClientManager.Dispose();
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
