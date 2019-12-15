using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ParMonitoring
{
    public class SshClientManager : IDisposable
    {
        private string sshHost;
        private string sshUser;
        private string sshPassword;
        private int sshPort = 22;

        private SshClient client;
        private PasswordConnectionInfo connectionInfo;

        public bool IsConnected
        {
            get
            {
                if (client == null) return false;
                return client.IsConnected;
            }
        }

        public SshClientManager(string sshHost, string sshUser, string sshPassword)
        {
            this.sshHost = sshHost;
            this.sshUser = sshUser;
            this.sshPassword = sshPassword;
        }
        public SshClientManager(string sshHost, string sshUser, string sshPassword, int sshPort)
        {
            this.sshHost = sshHost;
            this.sshUser = sshUser;
            this.sshPassword = sshPassword;
            this.sshPort = sshPort;
        }

        public bool Connect()
        {
            connectionInfo = new PasswordConnectionInfo(sshHost, sshPort, sshUser, sshPassword);
            //设置SSH连接超时时间
            connectionInfo.Timeout = TimeSpan.FromSeconds(10);
            client = new SshClient(connectionInfo);
            try
            {
                client.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't connect ssh server!");
                return false;
            }

            if (!client.IsConnected)
            {
                Console.WriteLine("Ssh service unreachable!");
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            if (client != null)
            {
                try
                {
                    client.Dispose();
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
        }
    }
}
